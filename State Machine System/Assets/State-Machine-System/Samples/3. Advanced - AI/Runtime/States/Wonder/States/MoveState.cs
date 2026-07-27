
namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class MoveState : TimedState<AICore>
    {
        public MoveState(StateDefinition definition) : base(definition) { }

        protected override void OnEnter()
        {
            Context.RandomizeMoveDirection();
            Context.MoveX();
        }

        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            Context.TryFlipMoveDirection();
            Context.MoveX();
        }
    }
}
