namespace M320.StateMachines
{
    public abstract class State<TContext> 
    {
        #region Fields & Properties

        public readonly TContext context;
        public readonly ITimeSource timeSource;

        public bool isFinished { get; protected set; }

        public float startTime { get; private set; }
        public float elapsedTime { get; private set; }
        public float fixedElapsedTime { get; private set; }

        public float currentTime => startTime + elapsedTime;
        public float fixedCurrentTime => startTime + fixedElapsedTime;

        #endregion

        public State(TContext context, ITimeSource timeSource) 
        {
            this.context = context;
            this.timeSource = timeSource;
        }

        public virtual void OnEnter()
        {
            isFinished = false;
            startTime = timeSource.Time;
            elapsedTime = 0f;
            fixedElapsedTime = 0;
        }

        public virtual void OnUpdate(float deltaTime) 
        {
            elapsedTime += deltaTime;
        }
        public virtual void OnFixedUpdate(float fixedDeltaTime)
        {
            fixedElapsedTime += fixedDeltaTime;
        }

        public virtual void OnExit() { }
    }
}
