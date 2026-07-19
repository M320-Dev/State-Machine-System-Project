using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace M320.StateMachineSystem.Samples.Timed
{
    public sealed class TimedCore : ContextlessStateMachineCore
    {
        public enum StateEnum 
        {
            Idle,
            Timed
        }

        #region Inspector Fields

        [SerializeField, Min(0f)] private float m_timedDuration = 1f;
        [SerializeField] private UpdateMode m_mode = UpdateMode.Update;

        #endregion

        #region Initialization

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new(StateEnum.Idle),
                new TimedState(StateEnum.Timed, m_timedDuration, m_mode)
            };
        }

        #endregion

        protected override void OnUpdate(float deltaTime)
        {
            Mouse mouse = Mouse.current;

            if (CurrentState.IsNamed(StateEnum.Idle))
            {
                if (mouse == null) return;
                if (!mouse.leftButton.wasPressedThisFrame) return;

                ChangeState(StateEnum.Timed);
            }
            else if (CurrentState.IsNamed(StateEnum.Timed))
            {
                ChangeState(StateEnum.Idle);
            }
        }
    }
}
