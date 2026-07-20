namespace M320.StateMachineSystem
{
    public class TimedState : State, ITimed
    {
        private bool _isFinished;
        public bool IsFinished => _isFinished;
        void IFinishable.SetIsFinished(bool isFinished) => _isFinished = isFinished;

        public float Duration { get; set; }
        public UpdateMode Mode { get; set; }

        #region Constructors

        public TimedState(StateName name, float duration, UpdateMode mode = UpdateMode.Update) 
            : base(name)
        {
            Duration = duration;
            Mode = mode;
        }
        public TimedState(StateName name, UpdateMode mode = UpdateMode.Update)
            : this(name, 0f, mode) { }
        public TimedState(float duration, UpdateMode mode = UpdateMode.Update) 
            : this("", duration, mode) { }
        public TimedState(UpdateMode mode = UpdateMode.Update)
            : this("", 0f, mode) { }

        #endregion

        public float NormalizedElapsedTime => this.NormalizedElapsedTime();

        protected override void OnReset()
        {
            base.OnReset();
            _isFinished = false;
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            this.TryFinish(UpdateMode.Update);
        }
        protected override void OnFixedUpdate(float fixedDeltaTime)
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

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
