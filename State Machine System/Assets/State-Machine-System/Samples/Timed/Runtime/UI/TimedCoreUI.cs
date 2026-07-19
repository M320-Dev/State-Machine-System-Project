using UnityEngine;
using UnityEngine.UI;

namespace M320.StateMachineSystem.Samples.Timed.UI
{
    public sealed class TimedCoreUI : MonoBehaviour
    {
        [SerializeField] private TimedCore m_core;
        [SerializeField] private Slider m_silder;

        private void Awake()
        {
            SetupSlider();
        }
        private void SetupSlider() 
        {
            if (m_silder == null) return;

            m_silder.minValue = 0f;
            m_silder.maxValue = 1f;
        }

        private void OnEnable()
        {
            m_core.StateChanged += OnStateChanged;
        }
        private void OnDisable()
        {
            m_core.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged(State previous, State current) 
        {
            OnStateChanged(current);
        }

        private void Start()
        {
            if (m_core == null) return;

            OnStateChanged(m_core.CurrentState);
        }

        private void Update()
        {
            OnUpdate(UpdateMode.Update);
        }
        private void FixedUpdate()
        {
            OnUpdate(UpdateMode.FixedUpdate);
        }

        private void OnUpdate(UpdateMode mode)
        {
            if (m_core == null || m_silder == null) return;

            if (m_core.CurrentState.IsNamed(TimedCore.StateEnum.Timed))
            {
                TimedState timedState = (TimedState)m_core.CurrentState;

                if (timedState.Mode != mode) return;

                m_silder.value = timedState.NormalizedElapsedTime;
            }
        }

        private void OnStateChanged(State current)
        {
            if (m_silder == null) return;

            if (current.IsNamed(TimedCore.StateEnum.Idle))
            {
                m_silder.value = 0f;
            }
            else if (current.IsNamed(TimedCore.StateEnum.Timed))
            {
                TimedState timedState = (TimedState)current;

                m_silder.value = timedState.NormalizedElapsedTime;
            }
        }
    }
}
