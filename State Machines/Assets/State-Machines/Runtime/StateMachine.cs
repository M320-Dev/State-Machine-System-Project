namespace M320.StateMachines
{
    public sealed class StateMachine<TContext>
    {
        public State<TContext> currentState { get; private set; }

        public StateMachine() { }
        public StateMachine(State<TContext> initialState) 
        {
            currentState = initialState;
            initialState?.OnEnter();
        }

        public void ChangeState(State<TContext> state, bool forceChange = true) 
        {
            if (state == null || state == currentState) return;

            if (!forceChange && currentState != null && !currentState.isFinished) return;

            currentState?.OnExit();
            currentState = state;
            currentState.OnEnter();
        }
    }
}
