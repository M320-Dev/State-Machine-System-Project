using System;

namespace M320.StateMachines
{
    public sealed class StateMachine
    {
        public State CurrentState { get; private set; }

        public StateMachine(State initialState, float startTime = 0f)
        {
            CurrentState = initialState ?? throw new ArgumentNullException(nameof(initialState));
            initialState?.OnEnter(startTime);
        }

        public void ChangeState(State state, float startTime = 0f, bool forceChange = false)
        {
            if (state == null || state == CurrentState) return;

            if (!forceChange && !CurrentState.IsFinished) return;

            CurrentState.OnExit();
            CurrentState = state;
            CurrentState.OnEnter(startTime);
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
