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

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new StopMovingXState(StateEnum.Alerted, Context.alerted),
                new PursueState(StateEnum.Pursue),
                new StopMovingXState(StateEnum.Failed, Context.failed),
                new StopMovingXState(StateEnum.Succeeded, Context.succeeded),
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
            if (CurrentState.IsNamed(StateEnum.Alerted))
            {
                ChangeState(StateEnum.Pursue);
            }
            else if (CurrentState.IsNamed(StateEnum.Pursue))
            {
                if (Context.isOverClosestVisibleTarget)
                {
                    Context.DisableClosestVisibleTarget();

                    ChangeState(StateEnum.Succeeded);
                }
                else if (!Context.seeingClosestVisibleTarget)
                {
                    ChangeState(StateEnum.Failed);
                }
            }
        }
        private void TryFinish() 
        {
            if (CurrentState.IsNamed(StateEnum.Failed) ||
                CurrentState.IsNamed(StateEnum.Succeeded))
            {
                var finishableState = (IFinishable)CurrentState;
                if (finishableState.IsFinished) _isFinished = true;
            }
        }
    }
}
