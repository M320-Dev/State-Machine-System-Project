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

            if (!forceChange && CurrentStateIsNotFinished()) return;

            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState.OnEnter();

            StateChanged?.Invoke(PreviousState, CurrentState);

            PreviousState = CurrentState;
        }
        private bool CurrentStateIsNotFinished() 
        {
            return CurrentState != null 
                && CurrentState is IFinishable finishable 
                && !finishable.IsFinished;
        }

        public void Update(float deltaTime)
        {
            CurrentState?.Update(deltaTime);
        }
        public void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState?.FixedUpdate(fixedDeltaTime);
        }
    }
}
