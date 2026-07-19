using System;
using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public sealed class StateMachine 
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

        #region Change State

        public bool ChangeState(State state, bool forceChange = false)
        {
            if (state == null || state == CurrentState) return false;

            if (!forceChange && !CurrentStateIsFinished()) return false;

            PreviousState = CurrentState;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();

            StateChanged?.Invoke(PreviousState, CurrentState);

            return true;
        }

        private bool CurrentStateIsFinished() 
        {
            return CurrentState == null 
                || CurrentState is not IFinishable finishable 
                || finishable.IsFinished;
        }

        #endregion

        #region Update

        public void Update(float deltaTime)
        {
            CurrentState?.Update(deltaTime);
        }
        public void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState?.FixedUpdate(fixedDeltaTime);
        }

        #endregion
    }
}
