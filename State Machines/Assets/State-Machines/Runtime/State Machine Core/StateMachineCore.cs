using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachines
{
    public abstract class StateMachineCoreBase : MonoBehaviour, IStateMachine
    {
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

        protected abstract State InitialState { get; }

        protected StateMachine Machine { get; private set; }
        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        #endregion

        #region Initialization

        protected virtual void Awake()
        {
            Initialize();
            Machine = new(InitialState);
        }

        protected virtual void Initialize() { }

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

        protected override void Initialize()
        {
            InitializationUtility.SetupStates(ConstructStates(), Context, StateDictionary);
        }

        protected abstract IReadOnlyList<State> ConstructStates();

        #endregion
    }
    public abstract class ContextlessStateMachineCore : StateMachineCore<EmptyContext>
    {
        protected override EmptyContext Context => default;
    }
}
