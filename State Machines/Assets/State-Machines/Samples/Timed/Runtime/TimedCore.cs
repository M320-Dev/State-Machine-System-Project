using UnityEngine;
using UnityEngine.InputSystem;

namespace M320.StateMachines.Samples.Platformer2D
{
    public sealed class TimedCore : StateMachineCore
    {
        [SerializeField, Min(0f)] private float m_timedDuration = 1f;
        [SerializeField] private UpdateMode m_mode = UpdateMode.Update;

        protected override IState InitialState => _idleState;

        private State _idleState;
        private TimedState _timedState;

        protected override void Initialize()
        {
            _idleState = new();
            _timedState = new(m_timedDuration, m_mode);
        }

        protected override void Update()
        {
            base.Update();

            Mouse mouse = Mouse.current;

            if (CurrentState == _idleState)
            {
                Debug.Log("Idle");

                if (mouse == null) return;
                if (!mouse.leftButton.wasPressedThisFrame) return;

                ChangeState(_timedState, true);
            }
            else if (CurrentState == _timedState)
            {
                Debug.Log("Timer");

                if (!_timedState.IsFinished) return;

                ChangeState(_idleState);
            }
        }
    }
}
