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

        #region States

        public State Idle { get; private set; }
        public TimedState Timed { get; private set; }

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

        protected override IEnumerable<StateDefinition> DefineChildren()
        {
            return new StateDefinition[]
            {
                StateDefinition.Create<State>(new(StateEnum.Idle)),
                StateDefinition.Create<TimedState>(new TimedStateParameters(StateEnum.Timed, m_timedDuration, m_mode))
            };
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (CurrentState == Idle)
            {
                if (!_buttonClickFlag) return;

                ChangeState(Timed);
            }
            else if (CurrentState == Timed)
            {
                ChangeState(Idle);
            }

            _buttonClickFlag = false;
        }
    }
}
