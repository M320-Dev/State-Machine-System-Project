using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachines.Samples.PlayerController
{
    public sealed class GroundedState : HierarchicalState
    {
        public enum States 
        {
            Idle,
            Move
        }

        private PlayerCore _playerCore;

        public GroundedState(StateName name, StateName initialStateName, float startTime = 0f) 
            : base(name, initialStateName, startTime) { }
        public GroundedState(StateName initialStateName, float startTime = 0f)
            : base(initialStateName, startTime) { }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new IdleState(States.Idle),
                new MoveState(States.Move)
            };
        }

        public override void OnEnter(float startTime)
        {
            base.OnEnter(startTime);

            _playerCore = (PlayerCore)Context;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            if (Mathf.Abs(_playerCore.rigidbody2D.linearVelocityX) <= 0.01f) ChangeState(States.Idle);
            else ChangeState(States.Move);

            _playerCore.TryJump();
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            _playerCore.TryMoveX();
        }
    }
}
