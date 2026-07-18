namespace M320.StateMachineSystem
{
    public class TimedState : State, ITimed
    {
        private bool _isFinished;
        public bool IsFinished => _isFinished;
        void IFinishable.SetIsFinished(bool isFinished) => _isFinished = isFinished;

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

            this.TryFinish(UpdateMode.Update);
        }
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            this.TryFinish(UpdateMode.FixedUpdate);
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
