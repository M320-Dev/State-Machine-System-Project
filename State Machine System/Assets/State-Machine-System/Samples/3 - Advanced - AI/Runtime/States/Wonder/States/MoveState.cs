
namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class MoveState : TimedState<AICore>
    {
        #region Constructors

        public MoveState(StateName name, float duration, UpdateMode mode = UpdateMode.Update)
            : base(name, duration, mode) { }
        public MoveState(StateName name, UpdateMode mode = UpdateMode.Update)
            : this(name, 0f, mode) { }
        public MoveState(float duration, UpdateMode mode = UpdateMode.Update)
            : this("", duration, mode) { }
        public MoveState(UpdateMode mode = UpdateMode.Update)
            : this("", 0f, mode) { }

        #endregion

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
