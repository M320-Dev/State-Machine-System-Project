using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachines
{
    public abstract class StateMachineCore : MonoBehaviour
    {
        #region Properties

        protected StateMachine Machine { get; private set; }
        protected State CurrentState => Machine.CurrentState;

        protected abstract State InitialState { get; }
        protected abstract object Context { get; }
        protected IReadOnlyList<State> States { get; private set; }

        #endregion

        protected virtual void Awake()
        {
            Initialize();
            Machine = new(InitialState);
        }

        protected virtual void Initialize() 
        {
            States = ConstructStates();
            foreach (var state in States)
            {
                state.Context = Context;
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
    }
}
