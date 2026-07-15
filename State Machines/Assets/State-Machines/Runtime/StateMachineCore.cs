using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachines
{
    public abstract class StateMachineCore : MonoBehaviour, IStateMachine
    {
        protected readonly StateDictionary StateDictionary = new();

        #region Properties

        protected StateMachine Machine { get; private set; }
        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        protected abstract State InitialState { get; }
        protected abstract object Context { get; }

        #endregion

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

        #region Initialization

        protected virtual void Awake()
        {
            Initialize();
            InitializationUtility.Initialize(ConstructStates(), Context, StateDictionary);
            Machine = new(InitialState);
        }

        protected virtual void Initialize() { }
        protected abstract IReadOnlyList<State> ConstructStates();

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
            Machine.ChangeState(state, Time.time, forceChange);
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
}
