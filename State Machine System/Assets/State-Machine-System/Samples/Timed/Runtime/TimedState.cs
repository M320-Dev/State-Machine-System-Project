using UnityEngine;

namespace M320.StateMachineSystem.Samples.Timed
{
    public sealed class TimedState : StateMachineSystem.TimedState
    {
        #region Constructors

        public TimedState(StateName name, float duration, UpdateMode mode = UpdateMode.Update)
            : base(name, duration, mode) { }
        public TimedState(float duration, UpdateMode mode = UpdateMode.Update)
            : base(duration, mode) { }

        #endregion

        protected override void OnEnter()
        {
            base.OnEnter();

            Debug.Log($"Started Timer | Duration: {Duration}");
        }

        protected override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            Debug.Log($"Updating Timer | Elapsed Time: {ElapsedTime} / {Duration}");
        }
        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            Debug.Log($"Updating Fixed Timer | Fixed Elapsed Time: {FixedElapsedTime} / {Duration}");
        }

        protected override void OnExit()
        {
            Debug.Log("Ended Timer");
        }
    }
}
