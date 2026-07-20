
namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class WonderMoveState : TimedState<AICore>
    {
        #region Constructors

        public WonderMoveState(StateName name, float duration, UpdateMode mode = UpdateMode.Update)
            : base(name, duration, mode) { }
        public WonderMoveState(StateName name, UpdateMode mode = UpdateMode.Update)
            : this(name, 0f, mode) { }
        public WonderMoveState(float duration, UpdateMode mode = UpdateMode.Update)
            : this("", duration, mode) { }
        public WonderMoveState(UpdateMode mode = UpdateMode.Update)
            : this("", 0f, mode) { }

        #endregion

        protected override void OnEnter()
        {
            Context.RandomizeMoveDirection();
        }

        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            Context.TryFlipMoveDirection();
            Context.MoveX();
        }

        protected override void OnExit()
        {
            Context.StopMovingX();
        }
    }
}
