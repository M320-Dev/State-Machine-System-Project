namespace M320.StateMachineSystem
{
    public class TimedState : State, ITimed
    {
        #region IFinishable

        private bool _isFinished;
        public bool IsFinished => _isFinished;
        void IFinishable.SetIsFinished(bool isFinished) => _isFinished = isFinished;

        #endregion

        new public readonly TimedStateParameters Parameters;

        public TimedState(TimedStateDefinition definition) : base(definition) 
        {
            Parameters = definition.parameters;
        }

        public float Duration
        {
            get => Parameters.duration;
            set => Parameters.duration = value;
        }
        public UpdateMode Mode
        {
            get => Parameters.mode;
            set => Parameters.mode = value;
        }

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

        public TimedState(TimedStateDefinition definition) : base(definition) { }

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
