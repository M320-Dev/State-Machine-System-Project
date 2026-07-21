namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class PursueState : State<AICore>
    {
        public PursueState(StateName name) : base(name) { }
        public PursueState() : base() { }

        protected override void OnFixedUpdate(float fixedDeltaTime)
        {
            Context.MoveTowardsTargetX();
        }
    }
}
