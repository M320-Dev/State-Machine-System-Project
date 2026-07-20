namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class ChaseState : State<AICore>
    {
        public enum StateEnum 
        {
            Pursue,
            Failed
        }

        public ChaseState(StateName name) : base(name) { }
        public ChaseState() : base() { }

        protected override void OnFixedUpdate(float deltaTime)
        {
            Context.SetMoveDirectionTarget();
            Context.MoveX();
        }
    }
}
