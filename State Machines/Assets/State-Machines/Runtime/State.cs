using System;

namespace M320.StateMachines
{
    public class State 
    {
        #region Properties

        public object Context { get; internal set; }

        public bool IsFinished { get; protected set; }

        public float StartTime { get; private set; }
        public float ElapsedTime { get; private set; }
        public float FixedElapsedTime { get; private set; }

        public float CurrentTime => StartTime + ElapsedTime;
        public float FixedCurrentTime => StartTime + FixedElapsedTime;

        public float GetElapsedTime(UpdateMode updateMode)
        {
            return updateMode switch
            {
                UpdateMode.Update => ElapsedTime,
                UpdateMode.FixedUpdate => FixedElapsedTime,
                _ => throw new ArgumentException("Invalid Update Mode!", nameof(updateMode))
            };
        }
        public float GetCurrentTime(UpdateMode updateMode)
        {
            return updateMode switch
            {
                UpdateMode.Update => CurrentTime,
                UpdateMode.FixedUpdate => FixedCurrentTime,
                _ => throw new ArgumentException("Invalid Update Mode!", nameof(updateMode))
            };
        }

        #endregion

        #region Methods

        public virtual void OnEnter(float startTime) 
        {
            IsFinished = false;
            StartTime = startTime;
            ElapsedTime = 0f;
            FixedElapsedTime = 0;
        }

        public virtual void OnUpdate(float deltaTime) 
        {
            ElapsedTime += deltaTime;
        }
        public virtual void OnFixedUpdate(float fixedDeltaTime) 
        {
            FixedElapsedTime += fixedDeltaTime;
        }

        public virtual void OnExit() { }

        #endregion
    }
}
