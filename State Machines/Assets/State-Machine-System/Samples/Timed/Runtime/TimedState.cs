using UnityEngine;

namespace M320.StateMachineSystem.Samples.Timed
{
    public sealed class TimedState : StateMachineSystem.TimedState
    {
        public TimedState(StateName name, float duration, UpdateMode mode) 
            : base(name, duration, mode) { }
        public TimedState(float duration, UpdateMode mode) 
            : base(duration, mode) { }

        public override void OnEnter()
        {
            base.OnEnter();

            Debug.Log($"Started Timer | Duration: {Duration}");
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            Debug.Log($"Updating Timer | Elapsed Time: {ElapsedTime} / {Duration}");
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
