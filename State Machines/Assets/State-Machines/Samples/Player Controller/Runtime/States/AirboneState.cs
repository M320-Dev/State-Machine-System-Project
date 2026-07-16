using System.Collections.Generic;

namespace M320.StateMachines.Samples.PlayerController
{
    public sealed class AirboneState : HierarchicalState<PlayerCore>
    {
        public enum States 
        {
            Rise,
            Fall
        }

        public AirboneState(StateName name, StateName initialStateName)
            : base(name, initialStateName) { }
        public AirboneState(StateName initialStateName)
            : base(initialStateName) { }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new(States.Rise),
                new(States.Fall)
            };
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            if (Context.rigidbody2D.linearVelocityY > 0f) ChangeState(States.Rise);
            else ChangeState(States.Fall);

            Context.TryMoveX();
        }
    }
}
