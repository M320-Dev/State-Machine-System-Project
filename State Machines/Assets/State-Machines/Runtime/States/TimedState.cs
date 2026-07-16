namespace M320.StateMachines
{
    public class TimedState : State
    {
        public override bool IsInitiallyFinished => false;

        public float Duration { get; }
        public UpdateMode Mode { get; }

        public TimedState(StateName name, float duration, UpdateMode mode) : base(name)
        {
            Duration = duration;
            Mode = mode;
        }
        public TimedState(float duration, UpdateMode mode)
            : this("", duration, mode) { }

        public float NormalizedElapsedTime => ElapsedTime / Duration;
        public float NormalizedFixedElapsedTime => FixedElapsedTime / Duration;
        public float GetNormalizedElapsedTime(UpdateMode mode) => GetElapsedTime(mode) / Duration;

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            TryFinish(UpdateMode.Update);
        }
        public override void OnFixedUpdate(float deltaTime)
        {
            base.OnFixedUpdate(deltaTime);
            TryFinish(UpdateMode.FixedUpdate);
        }

        private void TryFinish(UpdateMode mode)
        {
            if (Mode != mode) return;

            float elapsedTime = GetElapsedTime(mode);

            if (elapsedTime >= Duration) IsFinished = true;
        }
    }
    public class TimedState<TContext> : TimedState
    {
        new protected TContext Context;

        public TimedState(StateName name, float duration, UpdateMode mode) 
            : base(name, duration, mode) { }
        public TimedState(float duration, UpdateMode mode)
            : base(duration, mode) { }

        public override void SetupContext(object context)
        {
            base.SetupContext(context);
            Context = (TContext)context;
        }
    }
}
