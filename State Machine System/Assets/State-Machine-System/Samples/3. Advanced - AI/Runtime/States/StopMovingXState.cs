
namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class StopMovingXState : TimedState<AICore>
    {
        public StopMovingXState(StateDefinition definition) : base(definition) { }

        protected override void OnEnter()
        {
            Context.StopMovingX();
        }
    }
}
