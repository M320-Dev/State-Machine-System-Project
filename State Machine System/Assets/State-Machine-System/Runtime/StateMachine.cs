using System;
using UnityEngine;

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
            CurrentState?.Enter();
        }

        public void SetInitialState(State initialState) 
        {
            if (CurrentState != null) throw new InvalidOperationException("An initial state was already set!");

            CurrentState = initialState ?? throw new ArgumentNullException(nameof(initialState));
            PreviousState = CurrentState;
            CurrentState.Enter();
        }

        public void ChangeState(State state, bool forceChange = false)
        {
            if (state == null || state == CurrentState) return;

            Debug.Log($"{CurrentState.Name} | {CurrentStateIsFinished()} | {state.Name}");

            if (!forceChange && !CurrentStateIsFinished()) return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            StateChanged?.Invoke(PreviousState, CurrentState);

            PreviousState = CurrentState;
        }
        private bool CurrentStateIsFinished() 
        {
            return CurrentState == null 
                || CurrentState is not IFinishable finishable 
                || finishable.IsFinished;
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
