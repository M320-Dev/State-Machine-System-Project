namespace M320.StateMachines
{
    public interface ITimedStateProperties : IStateProperties
    {
        float Duration { get; }
        UpdateMode Mode { get; }
    }

    public abstract class TimedStateBase : StateBase, ITimedStateProperties
    {
        public float Duration { get; }
        public UpdateMode Mode { get; }

        public TimedStateBase(float duration, UpdateMode mode)
        {
            Duration = duration;
            Mode = mode;
        }

        protected void TryFinish(UpdateMode mode) 
        {
            if (Mode != mode) return;

            float elapsedTime = this.GetElapsedTime(mode);

            if (elapsedTime >= Duration) IsFinished = true;
        }
    }

    public class TimedState : TimedStateBase, IState
    {
        public TimedState(float duration, UpdateMode mode) : base(duration, mode) { }

        public virtual void OnEnter(float startTime) => Enter(startTime);

        public virtual void OnUpdate(float deltaTime)
        {
            UpdateElapsedTime(deltaTime);
            TryFinish(UpdateMode.Update);
        }
        public virtual void OnFixedUpdate(float deltaTime)
        {
            FixedUpdateElapsedTime(deltaTime);
            TryFinish(UpdateMode.FixedUpdate);
        }

        public virtual void OnExit() { }
    }
    public class TimedState<TContext> : TimedStateBase, IState<TContext>
    {
        public TimedState(float duration, UpdateMode mode) : base(duration, mode) { }

        public virtual void OnEnter(TContext context, float startTime) => Enter(startTime);

        public virtual void OnUpdate(TContext context, float deltaTime)
        {
            UpdateElapsedTime(deltaTime);
            TryFinish(UpdateMode.Update);
        }
        public virtual void OnFixedUpdate(TContext context, float deltaTime)
        {
            FixedUpdateElapsedTime(deltaTime);
            TryFinish(UpdateMode.FixedUpdate);
        }

        public virtual void OnExit(TContext context) { }
    }

    public static class TimedStateExtensions 
    {
        public static float NormalizedElapsedTime(this ITimedStateProperties state) 
        {
            return state.ElapsedTime / state.Duration;
        }
        public static float NormalizedFixedElapsedTime(this ITimedStateProperties state)
        {
            return state.FixedElapsedTime / state.Duration;
        }
        public static float GetNormalizedElapsedTime(this ITimedStateProperties state, UpdateMode mode)
        {
            return state.GetElapsedTime(mode) / state.Duration;
        }
    }
}
