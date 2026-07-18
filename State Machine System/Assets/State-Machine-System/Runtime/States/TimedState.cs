namespace M320.StateMachineSystem
{
    public class TimedState : State, ITimed
    {
        private bool _isFinished;
        public bool IsFinished => _isFinished;
        void IFinishable.SetIsFinished(bool isFinished) => _isFinished = isFinished;

        public float Duration { get; }
        public UpdateMode Mode { get; }

        #region Constructors

        public TimedState(StateName name, float duration, UpdateMode mode = UpdateMode.Update) 
            : base(name)
        {
            Duration = duration;
            Mode = mode;
        }
        public TimedState(float duration, UpdateMode mode = UpdateMode.Update) 
            : this("", duration, mode) { }

        #endregion

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

        #region Constructors

        public TimedState(StateName name, float duration, UpdateMode mode = UpdateMode.Update)
            : base(name, duration, mode) { }
        public TimedState(float duration, UpdateMode mode = UpdateMode.Update)
            : base(duration, mode) { }

        #endregion

        public override void SetupContext(object context)
        {
            base.SetupContext(context);
            Context = (TContext)context;
        }
    }
}
