using System;

namespace M320.StateMachines
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

        public StateMachine(State initialState, float startTime = 0f)
        {
            CurrentState = initialState ?? throw new ArgumentNullException(nameof(initialState));
            PreviousState = CurrentState;
            initialState?.OnEnter(startTime);
        }

        public void ChangeState(State state, float startTime = 0f, bool forceChange = false)
        {
            if (state == null || state == CurrentState) return;

            if (!forceChange && !CurrentState.IsFinished) return;

            CurrentState.OnExit();
            CurrentState = state;
            CurrentState.OnEnter(startTime);

            StateChanged?.Invoke(PreviousState, CurrentState);

            PreviousState = CurrentState;
        }

        public void Update(float deltaTime)
        {
            CurrentState.OnUpdate(deltaTime);
        }
        public void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState.OnFixedUpdate(fixedDeltaTime);
        }
    }
}
