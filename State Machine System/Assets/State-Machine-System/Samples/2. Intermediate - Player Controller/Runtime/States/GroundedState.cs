using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem.Samples.PlayerController
{
    public sealed class GroundedState : HierarchicalState<PlayerCore>
    {
        public enum StateEnum
        {
            Idle,
            Move
        }
        public GroundedState(StateName name) : base(name) { }
        public GroundedState() : base() { }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new(StateEnum.Idle),
                new(StateEnum.Move)
            };
        }

        protected override void OnUpdate(float deltaTime)
        {
            Context.TryJump();

            if (Mathf.Abs(Context.rigidbody2D.linearVelocityX) <= 0.01f) ChangeState(StateEnum.Idle);
            else ChangeState(StateEnum.Move);
        }

        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            Context.TryMoveX();
        }
    }
}
