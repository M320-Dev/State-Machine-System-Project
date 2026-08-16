using UnityEngine;

namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class WanderState : HierarchicalState<AICore>
    {
        public enum StateEnum 
        {
            Idle,
            Move
        }

        public WanderState(HierarchicalStateDefinition definition) : base(definition) => Suscribe();

        #region States

        public StopMovingXState Idle { get; private set; }
        public MoveState Move { get; private set; }

        #endregion

        private void Suscribe() 
        {
            StateChanged += OnStateChanged;
        }
        private void OnStateChanged(State previous, State current) 
        {
            if (current is TimedState timedState)
            {
                if (timedState == Idle) timedState.Duration = Context.RandomIdleDuration();
                else if (timedState == Move) timedState.Duration = Context.RandomMoveDuration();
            }
        }

       /* protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[] 
            {
                Idle = new StopMovingXState(new(StateEnum.Idle, Context.RandomIdleDuration())),
                Move = new MoveState(new(StateEnum.Move, Context.RandomMoveDuration()))
            };
        }*/

        protected override void OnEnter()
        {
            ChangeState(Random.value < 0.5f ? Idle : Move);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (CurrentState == Idle) ChangeState(Move);
            else if (CurrentState == Move) ChangeState(Idle);
        }
    }
}
