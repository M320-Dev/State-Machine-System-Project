namespace M320.StateMachines.Samples.PlayerController
{
    public sealed class MoveState : State
    {
        private PlayerCore _playerCore;

        public MoveState(StateName name) : base(name) { }
        public MoveState() : base() { }

        public override void OnEnter(float startTime)
        {
            base.OnEnter(startTime);

            _playerCore = (PlayerCore)Context;
        }
    }
}
