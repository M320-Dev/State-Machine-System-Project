using System;

namespace M320.StateMachines
{
    public interface IStateProperties 
    {
        public bool IsFinished { get; }

        public float StartTime { get; }
        public float ElapsedTime { get; }
        public float FixedElapsedTime { get; }
    }

    public interface IState : IStateProperties
    {
        void OnEnter(float startTime);

        void OnUpdate(float deltaTime);
        void OnFixedUpdate(float fixedDeltaTime);

        void OnExit();
    }
    public interface IState<TContext> : IStateProperties
    {
        void OnEnter(TContext context, float startTime);

        void OnUpdate(TContext context, float deltaTime);
        void OnFixedUpdate(TContext context, float fixedDeltaTime);

        void OnExit(TContext context);
    }

    public class StateBase : IStateProperties
    {
        #region Properties

        public bool IsFinished { get; protected set; }

        public float StartTime { get; private set; }
        public float ElapsedTime { get; private set; }
        public float FixedElapsedTime { get; private set; }

        #endregion

        #region Methods

        protected void Enter(float startTime)
        {
            IsFinished = false;
            StartTime = startTime;
            ElapsedTime = 0f;
            FixedElapsedTime = 0;
        }

        protected void UpdateElapsedTime(float deltaTime)
        {
            ElapsedTime += deltaTime;
        }
        protected void FixedUpdateElapsedTime(float fixedDeltaTime)
        {
            FixedElapsedTime += fixedDeltaTime;
        }

        #endregion
    }

    public class State : StateBase, IState
    {
        public virtual void OnEnter(float startTime) => Enter(startTime);

        public virtual void OnUpdate(float deltaTime) => UpdateElapsedTime(deltaTime);
        public virtual void OnFixedUpdate(float fixedDeltaTime) => FixedUpdateElapsedTime(fixedDeltaTime);

        public virtual void OnExit() { }
    }
    public class State<TContext> : StateBase, IState<TContext>
    {
        public virtual void OnEnter(TContext context, float startTime) => Enter(startTime);

        public virtual void OnUpdate(TContext context, float deltaTime) => UpdateElapsedTime(deltaTime);
        public virtual void OnFixedUpdate(TContext context, float fixedDeltaTime) => FixedUpdateElapsedTime(fixedDeltaTime);

        public virtual void OnExit(TContext context) { }
    }

    public static class StateExtensions
    {
        public static float CurrentTime(this IStateProperties state)
        {
            return state.StartTime + state.ElapsedTime;
        }
        public static float FixedCurrentTime(this IStateProperties state)
        {
            return state.StartTime + state.FixedElapsedTime;
        }

        public static float GetElapsedTime(this IStateProperties state, UpdateMode updateMode)
        {
            return updateMode switch
            {
                UpdateMode.Update => state.ElapsedTime,
                UpdateMode.FixedUpdate => state.FixedElapsedTime,
                _ => throw new ArgumentException("Invalid Update Mode!", nameof(updateMode))
            };
        }
        public static float GetCurrentTime(this IStateProperties state, UpdateMode updateMode)
        {
            return updateMode switch
            {
                UpdateMode.Update => CurrentTime(state),
                UpdateMode.FixedUpdate => FixedCurrentTime(state),
                _ => throw new ArgumentException("Invalid Update Mode!", nameof(updateMode))
            };
        }
    }
}
