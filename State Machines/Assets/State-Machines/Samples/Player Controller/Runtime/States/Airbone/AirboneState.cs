namespace M320.StateMachines.Samples.PlayerController
{
    public sealed class AirboneState : State
    {
        private PlayerCore _playerCore;

        public AirboneState(StateName name) : base(name) { }
        public AirboneState() : base() { }

        public override void OnEnter(float startTime)
        {
            base.OnEnter(startTime);

            _playerCore = (PlayerCore)Context;
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            _playerCore.TryMoveX();
        }
    }
}
