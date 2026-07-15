using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace M320.StateMachines.Samples.Platformer2D
{
    public sealed class TimedCore : StateMachineCore
    {
        #region Inspector Fields

        [SerializeField, Min(0f)] private float m_timedDuration = 1f;
        [SerializeField] private UpdateMode m_mode = UpdateMode.Update;

        #endregion

        #region States

        private IdleState _idleState;
        private TimedState _timedState;

        #endregion

        #region Properties

        protected override State InitialState => _idleState;
        protected override object Context => null;

        #endregion

        #region Initialization

        protected override void Initialize()
        {
            _idleState = new("Idle");
            _timedState = new("Timed", m_timedDuration, m_mode);
        }
        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                _idleState,
                _timedState
            };
        }

        #endregion

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
