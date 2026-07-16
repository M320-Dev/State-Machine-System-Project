#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachines
{
    public sealed class StateMachineCoreDebugger : MonoBehaviour
    {
        #region Inspector Fields

        [Header("Component")]
        [SerializeField] private StateMachineCoreBase m_core;

        [Header("Style Settings")]
        [SerializeField] private string m_separator = " => ";
        [SerializeField] private Color m_separatorColor = Color.yellow;
        [SerializeField] private Color m_stateNameColor = Color.white;

        #endregion

        private void Update()
        {
            List<State> currentStateBranch = m_core.GetCurrentStateBranch();

            string s = StateName(currentStateBranch[0]);
            string seperator = Separator();

            for (int i = 1; i < currentStateBranch.Count; i++)
            {
                s += seperator + StateName(currentStateBranch[i]);
            }

            Debug.Log(s);
        }

        #region Helper Methods

        private string Separator() 
        {
            return ColorText(m_separator, m_separatorColor);
        }
        private string StateName(State state)
        {
            return ColorText(state.Name, m_stateNameColor);
        }
        private string ColorText(string s, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{s}</color>";
        }

        #endregion
    }
}

#endif