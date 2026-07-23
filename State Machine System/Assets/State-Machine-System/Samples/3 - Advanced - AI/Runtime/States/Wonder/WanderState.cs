using System.Collections.Generic;
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

        public WanderState(StateName name) : base(name) => Suscribe();
        public WanderState() : base() => Suscribe();

        private void Suscribe() 
        {
            StateChanged += OnStateChanged;
        }
        private void OnStateChanged(State previous, State current) 
        {
            if (current is TimedState timedState)
            {
                if (timedState.IsNamed(StateEnum.Idle)) timedState.Duration = Context.RandomIdleDuration();
                if (timedState.IsNamed(StateEnum.Move)) timedState.Duration = Context.RandomMoveDuration();
            }
        }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[] 
            {
                new StopMovingXState(StateEnum.Idle, Context.RandomIdleDuration(), UpdateMode.Update),
                new MoveState(StateEnum.Move, Context.RandomMoveDuration(), UpdateMode.Update)
            };
        }

        protected override void OnEnter()
        {
            ChangeState(
                Random.value < 0.5f 
                ? StateEnum.Idle 
                : StateEnum.Move);
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (CurrentState.IsNamed(StateEnum.Idle))
            {
                ChangeState(StateEnum.Move);
            }
            else if (CurrentState.IsNamed(StateEnum.Move))
            {
                ChangeState(StateEnum.Idle);
            }
        }
    }
}
