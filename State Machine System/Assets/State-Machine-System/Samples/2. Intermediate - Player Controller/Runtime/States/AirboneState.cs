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

        public AirboneState(StateName name) : base(name) { }
        public AirboneState() : base() { }

        #region States

        public State Rise { get; private set; }
        public State Fall { get; private set; }

        #endregion

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                Rise = new(StateEnum.Rise),
                Fall = new(StateEnum.Fall)
            };
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (Context.rigidbody2D.linearVelocityY > 0f) ChangeState(Rise);
            else ChangeState(Fall);
        }

        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            Context.TryMoveX();
        }
    }
}
