namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class PursueState : State<AICore>
    {
        public PursueState(StateDefinition definition) : base(definition) { }

        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            Context.MoveTowardsTargetX();
        }
    }
}
