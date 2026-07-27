using System;

namespace M320.StateMachineSystem
{
    public interface IReadOnlyTimed : IFinishable
    {
        float Duration { get; }
        UpdateMode Mode { get; }
    }
    public interface ITimed : IReadOnlyTimed
    {
        float IReadOnlyTimed.Duration => Duration;
        UpdateMode IReadOnlyTimed.Mode => Mode;

        new float Duration { get; set; }
        new UpdateMode Mode { get; set; }
    }

    public static class ITimedExtension 
    {
        public static float NormalizedElapsedTime<TTimedState>(this TTimedState timedState) 
            where TTimedState : State, ITimed
        {
            float normalizedElapsedTime = timedState.GetElapsedTime(timedState.Mode) / timedState.Duration;
            normalizedElapsedTime = MathF.Min(normalizedElapsedTime, 1f);
            return normalizedElapsedTime;
        }
        public static void TryFinish<TTimedState>(this TTimedState timedState, UpdateMode mode)
            where TTimedState : State, ITimed
        {
            if (timedState.Mode != mode) return;

            if (timedState.GetElapsedTime(mode) >= timedState.Duration) timedState.SetIsFinished(true);
        }
    }
}
