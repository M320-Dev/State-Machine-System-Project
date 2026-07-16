using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachines.Samples.PlayerController
{
    public sealed class GroundedState : HierarchicalState<PlayerCore>
    {
        public enum StateEnum
        {
            Idle,
            Move
        }
        public GroundedState(StateName name, StateName initialStateName) 
            : base(name, initialStateName) { }
        public GroundedState(StateName name)
           : base(name) { }
        public GroundedState()
            : base() { }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new(StateEnum.Idle),
                new(StateEnum.Move)
            };
        }

        public override void OnEnter()
        {
            base.OnEnter();

            ChangeState();
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            ChangeState();

            Context.TryJump();
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            Context.TryMoveX();
        }

        private void ChangeState() 
        {
            if (Mathf.Abs(Context.rigidbody2D.linearVelocityX) <= 0.01f) ChangeState(StateEnum.Idle);
            else ChangeState(StateEnum.Move);
        }
    }
}
