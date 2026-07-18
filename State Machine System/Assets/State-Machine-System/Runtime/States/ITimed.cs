namespace M320.StateMachineSystem
{
    public interface ITimed : IFinishable
    {
        float Duration { get; }
        UpdateMode Mode { get; }
    }

    public static class ITimedExtension 
    {
        public static float NormalizedElapsedTime<TTimedState>(this TTimedState timedState) 
            where TTimedState : State, ITimed
        {
            return timedState.GetElapsedTime(timedState.Mode) / timedState.Duration;
        }
        public static void TryFinish<TTimedState>(this TTimedState timedState, UpdateMode mode)
            where TTimedState : State, ITimed
        {
            if (timedState.Mode != mode) return;

            if (timedState.GetElapsedTime(mode) >= timedState.Duration) timedState.SetIsFinished(true);
        }
    }
}
