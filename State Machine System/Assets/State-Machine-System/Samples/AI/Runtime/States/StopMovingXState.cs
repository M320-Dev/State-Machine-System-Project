
namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class StopMovingXState : TimedState<AICore>
    {
        #region Constructors

        public StopMovingXState(StateName name, float duration, UpdateMode mode = UpdateMode.Update)
            : base(name, duration, mode) { }
        public StopMovingXState(StateName name, UpdateMode mode = UpdateMode.Update)
            : this(name, 0f, mode) { }
        public StopMovingXState(float duration, UpdateMode mode = UpdateMode.Update)
            : this("", duration, mode) { }
        public StopMovingXState(UpdateMode mode = UpdateMode.Update)
            : this("", 0f, mode) { }

        #endregion

        protected override void OnEnter()
        {
            Context.StopMovingX();
        }
    }
}
