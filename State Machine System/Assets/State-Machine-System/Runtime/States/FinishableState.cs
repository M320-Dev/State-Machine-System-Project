namespace M320.StateMachineSystem
{
    public class FinishableState : State, IFinishable
    {
        private bool _isFinished;
        public bool IsFinished => _isFinished;
        void IFinishable.SetIsFinished(bool isFinished) => _isFinished = isFinished;

        #region Constructors

        public FinishableState(StateName name) : base(name) { }
        public FinishableState() : base() { }

        #endregion
    }
    public class FinishableState<TContext> : FinishableState
    {
        new protected TContext Context;

        #region Constructors

        public FinishableState(StateName name) : base(name) { }
        public FinishableState() : base() { }

        #endregion

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
