using System.Collections.Generic;

namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class ChaseState : HierarchicalState<AICore>, IFinishable
    {
        public enum StateEnum
        {
            Alerted,
            Pursue,
            Failed,
            Succeeded
        }

        private bool _isFinished;
        public bool IsFinished => _isFinished;
        void IFinishable.SetIsFinished(bool isFinished)
        {
            _isFinished = isFinished;
        }

        public ChaseState(StateName name) : base(name) { }
        public ChaseState() : base() { }

        #region States

        public StopMovingXState Alerted { get; private set; }
        public PursueState Pursue { get; private set; }
        public StopMovingXState Failed { get; private set; }
        public StopMovingXState Succeeded { get; private set; }

        #endregion

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                Alerted = new(StateEnum.Alerted, Context.alerted),
                Pursue = new(StateEnum.Pursue),
                Failed = new(StateEnum.Failed, Context.failed),
                Succeeded = new(StateEnum.Succeeded, Context.succeeded),
            };
        }

        protected override void OnReset()
        {
            base.OnReset();

            _isFinished = false;
        }

        protected override void OnEnter()
        {
            ChangeState(StateEnum.Alerted, true);
        }

        protected override void OnUpdate(float deltaTime)
        {
            TryChangeState();
            TryFinish();
        }

        private void TryChangeState() 
        {
            if (CurrentState == Alerted)
            {
                ChangeState(Pursue);
            }
            else if (CurrentState == Pursue)
            {
                if (Context.isOverClosestVisibleTarget)
                {
                    Context.DisableClosestVisibleTarget();

                    ChangeState(Succeeded);
                }
                else if (!Context.seeingClosestVisibleTarget)
                {
                    ChangeState(Failed);
                }
            }
        }
        private void TryFinish() 
        {
            if (CurrentState == Failed || CurrentState == Succeeded)
            {
                var finishableState = (IFinishable)CurrentState;
                if (finishableState.IsFinished) _isFinished = true;
            }
        }
    }
}
