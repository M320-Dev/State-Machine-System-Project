namespace M320.StateMachineSystem
{
    public interface IReadOnlyFinishable 
    {
        bool IsFinished { get; }
    }
    public interface IFinishable : IReadOnlyFinishable
    {
        internal void SetIsFinished(bool isFinished);
    }
}
