namespace M320.StateMachines
{
    public sealed class StateMachine
    {
        public IState CurrentState { get; private set; }

        public StateMachine() { }
        public StateMachine(IState initialState, float startTime = 0f)
        {
            CurrentState = initialState;
            initialState?.OnEnter(startTime);
        }

        public void ChangeState(IState state, float startTime = 0f, bool forceChange = false)
        {
            if (!StateMachineHelper.CanChangeState(CurrentState, state, forceChange)) return;

            CurrentState?.OnExit();
            CurrentState = state;
            CurrentState.OnEnter(startTime);
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

    public sealed class StateMachine<TContext>
    {
        public readonly TContext Context;
        public IState<TContext> CurrentState { get; private set; }

        public StateMachine(TContext context) 
        {
            Context = context;
        }
        public StateMachine(TContext context, IState<TContext> initialState, float startTime = 0f) 
        {
            Context = context;
            CurrentState = initialState;
            initialState?.OnEnter(context, startTime);
        }

        public void ChangeState(IState<TContext> state, float startTime = 0f, bool forceChange = false) 
        {
            if (!StateMachineHelper.CanChangeState(CurrentState, state, forceChange)) return;

            CurrentState?.OnExit(Context);
            CurrentState = state;
            CurrentState.OnEnter(Context, startTime);
        }

        public void Update(float deltaTime) 
        {
            CurrentState?.OnUpdate(Context, deltaTime);
        }
        public void FixedUpdate(float fixedDeltaTime)
        {
            CurrentState?.OnFixedUpdate(Context, fixedDeltaTime);
        }
    }

    internal static class StateMachineHelper
    {
        public static bool CanChangeState(IStateProperties currentState, IStateProperties newState, bool forceChange = false) 
        {
            if (newState == null || newState == currentState) return false;

            if (!forceChange && currentState != null && !currentState.IsFinished) return false;

            return true;
        }
    }
}
