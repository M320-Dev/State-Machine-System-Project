using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Button m_button;

        #endregion

        private bool _buttonClickFlag;

        #region Subscription

        private void OnEnable()
        {
            m_button.onClick.AddListener(OnButtonClick);
        }
        private void OnDisable()
        {
            m_button.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick() 
        {
            _buttonClickFlag = true;
        }

        #endregion

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new(StateEnum.Idle),
                new TimedState(StateEnum.Timed, m_timedDuration, m_mode)
            };
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (CurrentState.IsNamed(StateEnum.Idle))
            {
                if (!_buttonClickFlag) return;

                ChangeState(StateEnum.Timed);
            }
            else if (CurrentState.IsNamed(StateEnum.Timed))
            {
                ChangeState(StateEnum.Idle);
            }

            _buttonClickFlag = false;
        }
    }
}
