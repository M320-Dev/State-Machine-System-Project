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

        protected virtual void Awake()
        {
            Initialize();
            Machine = new(InitialState);
        }

        protected virtual void Initialize() 
        {
            foreach (var state in ConstructStates())
            {
                state.Context = Context;
                StateDictionary.Add(state);
            }
        }
        protected abstract IReadOnlyList<State> ConstructStates();

        protected virtual void Update()
        {
            Machine.Update(Time.deltaTime);
        }
        protected virtual void FixedUpdate()
        {
            Machine.FixedUpdate(Time.fixedDeltaTime);
        }

        protected void ChangeState(State state, bool forceChange = false)
        {
            Machine.ChangeState(state, Time.time, forceChange);
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
}
