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
        public GroundedState(HierarchicalStateDefinition definition) : base(definition) { }

        #region States

        public State Idle { get; private set; }
        public State Move { get; private set; }

        #endregion

        /*protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                Idle = new(new(StateEnum.Idle)),
                Move = new(new(StateEnum.Move))
            };
        }*/

        protected override void OnUpdate(float deltaTime)
        {
            Context.TryJump();

            if (Mathf.Abs(Context.rigidbody2D.linearVelocityX) <= 0.01f) ChangeState(Idle);
            else ChangeState(Move);
        }

        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            Context.TryMoveX();
        }
    }
}
