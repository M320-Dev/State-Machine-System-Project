namespace M320.StateMachineSystem
{
    public interface IReadOnlyFinishable 
    {
        bool IsFinished { get; }
    }
    public interface IFinishableInternal : IReadOnlyFinishable
    {
        internal void SetIsFinished(bool isFinished);
    }
    public interface IFinishable : IFinishableInternal
    {
        void IFinishableInternal.SetIsFinished(bool isFinished) => SetIsFinished(isFinished);
        new protected void SetIsFinished(bool isFinished);
    }
}
