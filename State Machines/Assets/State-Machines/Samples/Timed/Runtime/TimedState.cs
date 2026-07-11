using UnityEngine;

namespace M320.StateMachines.Samples.Platformer2D
{
    public sealed class TimedState : StateMachines.TimedState
    {
        public TimedState(float Duration, UpdateMode mode) : base(Duration, mode) { }

        public override void OnEnter(float startTime)
        {
            base.OnEnter(startTime);

            Debug.Log($"Started Timer | Duration: {Duration}");
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            Debug.Log($"Updating Timer | Elapsed Time: {ElapsedTime} / {Duration}");

            if (ElapsedTime >= Duration) IsFinished = true;
        }
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            Debug.Log($"Updating Fixed Timer | Fixed Elapsed Time: {FixedElapsedTime} / {Duration}");
        }

        public override void OnExit()
        {
            Debug.Log("Ended Timer");
        }
    }
}
