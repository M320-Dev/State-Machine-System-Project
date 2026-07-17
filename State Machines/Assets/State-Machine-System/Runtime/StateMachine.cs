using System;

namespace M320.StateMachineSystem
{
    public interface IStateMachine
    {
        State PreviousState { get; }
        State CurrentState { get; }

        event ChangeHandler<State> StateChanged;
    }

    public sealed class StateMachine : IStateMachine
    {
        public State PreviousState { get; private set; }
        public State CurrentState { get; private set; }

        public event ChangeHandler<State> StateChanged;

        public StateMachine(State initialState = null)
        {
            CurrentState = initialState;
            PreviousState = CurrentState;
            CurrentState?.OnEnter();
        }

        public void SetInitialState(State initialState) 
        {
            if (CurrentState != null) throw new InvalidOperationException("An initial state was already set!");

            CurrentState = initialState ?? throw new ArgumentNullException(nameof(initialState));
            PreviousState = CurrentState;
            CurrentState.OnEnter();
        }

        public void ChangeState(State state, bool forceChange = false)
        {
            if (state == null || state == CurrentState) return;

            if (!forceChange && CurrentState != null && !CurrentState.IsFinished) return;

            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState.OnEnter();

            StateChanged?.Invoke(PreviousState, CurrentState);

            PreviousState = CurrentState;
        }

        public void Update(float deltaTime)
        {
            CurrentState?.OnUpdate(deltaTime);
        }
        public void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState?.OnFixedUpdate(fixedDeltaTime);
        }
    }
}
