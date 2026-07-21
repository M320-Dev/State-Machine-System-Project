#if UNITY_EDITOR

using TMPro;
using UnityEditor;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public sealed class StateMachineCoreDebuggerUI : StateMachineCoreDebuggerBase
    {
        [Header("TMP")]
        [SerializeField] private TextMeshProUGUI m_tmp;

        private void OnValidate()
        {
            UpdateTMP();
        }

        private void Update()
        {
            UpdateTMP();
        }

        private void UpdateTMP() 
        {
            if (!m_tmp) return;

            m_tmp.richText = true;
            m_tmp.text = CurrentStateBranchString();
        }
    }
}

#endif