using System;
using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public abstract class StateMachineCoreBase : MonoBehaviour
    {
        [SerializeField] private string m_initialLeafState;

        protected readonly StateDictionary StateDictionary = new();

        public event ChangeHandler<State> StateChanged;

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
            Machine.StateChanged += (p, c) => StateChanged?.Invoke(p, c);
        }

        /// <returns>
        /// Initial State
        /// </returns>
        protected abstract State Initialize();

        public State GetState(StateName name) 
        {
            return StateDictionary[name];
        }

        #endregion

        #region Update

        protected virtual void Update()
        {
            float deltaTime = Time.deltaTime;
            OnUpdate(deltaTime);
            Machine.Update(deltaTime);
        }
        protected virtual void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            OnFixedUpdate(fixedDeltaTime);
            Machine.FixedUpdate(Time.fixedDeltaTime);
        }

        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnFixedUpdate(float fixedDeltaTime) { }

        #endregion

        #region Change State

        public bool ChangeState(StateName stateName, bool forceChange = false)
        {
            return ChangeState(StateDictionary[stateName], forceChange);
        }
        public bool ChangeState(State state, bool forceChange = false)
        {
            return Machine.ChangeState(state, forceChange);
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
            return Machine.GetCurrentStateBranch();
        }
    }
    public abstract class StateMachineCore<TContext> : StateMachineCoreBase
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
