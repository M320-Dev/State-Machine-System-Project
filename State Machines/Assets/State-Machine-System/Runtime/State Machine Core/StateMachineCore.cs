using System;
using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public abstract class StateMachineCoreBase : MonoBehaviour, IStateMachine
    {
        [SerializeField] private string m_initialLeafState;

        protected readonly StateDictionary StateDictionary = new();

        public event ChangeHandler<State> StateChanged
        {
            add
            {
                if (Machine != null) Machine.StateChanged += value;
            }
            remove
            {
                if (Machine != null) Machine.StateChanged -= value;
            }
        }

        #region Properties

        public IReadOnlyCollection<State> States => StateDictionary.States;

        protected StateMachine Machine { get; private set; }
        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        #endregion

        #region Initialization

        protected virtual void Awake()
        {
            State initialState = Initialize();
            Machine = new(initialState);
        }

        /// <returns>
        /// Initial State
        /// </returns>
        protected abstract State Initialize();

        #endregion

        #region Update

        protected virtual void Update()
        {
            Machine.Update(Time.deltaTime);
        }
        protected virtual void FixedUpdate()
        {
            Machine.FixedUpdate(Time.fixedDeltaTime);
        }

        #endregion

        #region Change State

        protected void ChangeState(StateName stateName, bool forceChange = false)
        {
            ChangeState(StateDictionary[stateName], forceChange);
        }
        protected void ChangeState(State state, bool forceChange = false)
        {
            Machine.ChangeState(state, forceChange);
        }

        #endregion

        protected (List<HierarchicalState> ancestryBranch, State leaf) GetInitialStateBranch()
        {
            State foundState = null;

            foreach (var state in States)
            {
                Helper(state);
            }

            if (foundState == null)
            {
                throw new InvalidOperationException(
                    "Initial State Branch not found! " +
                    "The initial state must be a leaf and must exist!");
            }

            return (foundState.GetAncestryBranch(), foundState);

            void Helper(State state)
            {
                if (foundState != null) return;

                if (state is not HierarchicalState hierarchicalState)
                {
                    if (state.Name == m_initialLeafState) foundState = state;

                    return;
                }

                foreach (var _state in hierarchicalState.States)
                {
                    Helper(_state);
                }
            }
        }

        public List<State> GetCurrentStateBranch()
        {
            List<State> stateBranch = new();
            State state = CurrentState;
            while (state != null)
            {
                stateBranch.Add(state);

                if (state is not HierarchicalState hierarchicalState) break;

                state = hierarchicalState.CurrentState;
            }
            return stateBranch;
        }
    }
    public abstract class StateMachineCore<TContext> : StateMachineCoreBase, IStateMachine
    {
        protected abstract TContext Context { get; }

        #region Initialization

        protected override State Initialize()
        {
            InitializationUtility.SetupStates(ConstructStates(), Context, StateDictionary);

            var (ancestryBranch, leaf) = GetInitialStateBranch();
            InitializationUtility.SetupInitialStateBranch(ancestryBranch, leaf);

            return ancestryBranch.Count > 0 ? ancestryBranch[0] : leaf;
        }

        protected abstract IReadOnlyList<State> ConstructStates();

        #endregion
    }
    public abstract class ContextlessStateMachineCore : StateMachineCore<EmptyContext>
    {
        protected override EmptyContext Context => default;
    }
}
