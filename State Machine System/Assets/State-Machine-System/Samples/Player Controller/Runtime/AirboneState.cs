using System.Collections.Generic;

namespace M320.StateMachineSystem.Samples.PlayerController
{
    public sealed class AirboneState : HierarchicalState<PlayerCore>
    {
        public enum StateEnum
        {
            Rise,
            Fall
        }

        public AirboneState(StateName name)
            : base(name) { }
        public AirboneState()
            : base() { }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new(StateEnum.Rise),
                new(StateEnum.Fall)
            };
        }

        public override void OnUpdate(float deltaTime)
        {
            if (Context.rigidbody2D.linearVelocityY > 0f) ChangeState(StateEnum.Rise);
            else ChangeState(StateEnum.Fall);
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            Context.TryMoveX();
        }
    }
}
