using UnityEngine;

namespace M320.StateMachines
{
    public abstract class StateMachineCore : MonoBehaviour
    {
        #region Properties

        protected abstract IState InitialState { get; }
        protected StateMachine Machine { get; private set; }
        protected IState CurrentState => Machine.CurrentState;

        #endregion

        private void Awake()
        {
            Initialize();
            Machine = new(InitialState, 0f);
        }

        protected virtual void Initialize() { }

        protected virtual void Update()
        {
            Machine.Update(Time.deltaTime);
        }
        protected virtual void FixedUpdate()
        {
            Machine.FixedUpdate(Time.fixedDeltaTime);
        }

        protected void ChangeState(IState state, bool forceChange = false)
        {
            Machine.ChangeState(state, Time.time, forceChange);
        }
    }

    public abstract class StateMachineCore<TContext> : MonoBehaviour
    {
        #region Properties

        protected abstract TContext Context { get; }
        protected abstract IState<TContext> InitialState { get; }
        protected StateMachine<TContext> Machine { get; private set; }
        protected IState<TContext> CurrentState => Machine.CurrentState;

        #endregion

        private void Awake()
        {
            Initialize();
            Machine = new(Context, InitialState, 0f);
        }

        protected virtual void Initialize() { }

        protected virtual void Update()
        {
            Machine.Update(Time.deltaTime);
        }
        protected virtual void FixedUpdate()
        {
            Machine.FixedUpdate(Time.fixedDeltaTime);
        }

        protected void ChangeState(IState<TContext> state, bool forceChange = false) 
        {
            Machine.ChangeState(state, Time.time, forceChange);
        }
    }
}
