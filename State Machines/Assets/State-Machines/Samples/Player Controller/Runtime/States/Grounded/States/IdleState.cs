namespace M320.StateMachines.Samples.PlayerController
{
    public sealed class IdleState : State
    {
        private PlayerCore _playerCore;

        public IdleState(StateName name) : base(name) { }
        public IdleState() : base() { }

        public override void OnEnter(float startTime)
        {
            base.OnEnter(startTime);

            _playerCore = (PlayerCore)Context;
        }
    }
}
