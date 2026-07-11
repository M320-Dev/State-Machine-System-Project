namespace M320.StateMachines
{
    public class TimedState : State
    {
        public float Duration { get; }
        public UpdateMode Mode { get; }

        public TimedState(float duration, UpdateMode mode)
        {
            Duration = duration;
            Mode = mode;
        }

        public float NormalizedElapsedTime => ElapsedTime / Duration;
        public float NormalizedFixedElapsedTime => FixedElapsedTime / Duration;
        public float GetNormalizedElapsedTime(UpdateMode mode) => GetElapsedTime(mode) / Duration;

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            TryFinish(UpdateMode.Update);
        }
        public override void OnFixedUpdate(float deltaTime)
        {
            base.OnFixedUpdate(deltaTime);
            TryFinish(UpdateMode.FixedUpdate);
        }

        private void TryFinish(UpdateMode mode)
        {
            if (Mode != mode) return;

            float elapsedTime = GetElapsedTime(mode);

            if (elapsedTime >= Duration) IsFinished = true;
        }
    }
}
