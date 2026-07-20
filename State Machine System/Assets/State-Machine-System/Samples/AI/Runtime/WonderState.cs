using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class WonderState : HierarchicalState<AICore>
    {
        public enum StateEnum 
        {
            Idle,
            Move
        }

        public WonderState(StateName name) : base(name) => Suscribe();
        public WonderState() : base() => Suscribe();

        private void Suscribe() 
        {
            StateChanged += OnStateChanged;
        }
        private void OnStateChanged(State previous, State current) 
        {
            if (current is TimedState timedState) 
            {
                timedState.Duration = RandomDuration();
            }
        }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[] 
            {
                new TimedState(StateEnum.Idle, RandomDuration(), UpdateMode.Update),
                new WonderMoveState(StateEnum.Move, RandomDuration(), UpdateMode.Update)
            };
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

        public float RandomDuration() 
        {
            return Random.Range(0.5f, 2f);
        }
    }
}
