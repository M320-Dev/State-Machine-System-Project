#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public sealed class StateMachineCoreDebugger : MonoBehaviour
    {
        #region Inspector Fields

        [Header("Component")]
        [SerializeField] private StateMachineCoreBase m_core;

        [Header("Style Settings")]
        [SerializeField] private string m_separator = "=>";
        [SerializeField] private Color m_separatorColor = Color.yellow;
        [SerializeField] private Color m_stateNameColor = Color.white;
        [SerializeField] private Color m_warningColor = Color.red;

        #endregion

        private void OnDrawGizmos()
        {
            string s = CurrentStateBranchString();
            GUIStyle style = Style();
            Handles.Label(transform.position, s, style);
        }

        private GUIStyle Style() 
        {
            GUIStyle style = GUI.skin.label;
            style.richText = true;
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }

        private string CurrentStateBranchString() 
        {
            if (m_core == null) return NoStateMachineCore();

            List<State> currentStateBranch = m_core.GetCurrentStateBranch();
            if (currentStateBranch == null || currentStateBranch.Count == 0) return NoCurrentStateBranch();

            string s = StateName(currentStateBranch[0]);
            string seperator = Separator();

            for (int i = 1; i < currentStateBranch.Count; i++)
            {
                s += seperator + StateName(currentStateBranch[i]);
            }

            return s;
        }

        #region Helper Methods

        private string NoStateMachineCore()
        {
            return ColorText("NO STATE MACHINE CORE!", m_warningColor);
        }
        private string NoCurrentStateBranch()
        {
            return ColorText("NO CURRENT STATE BRANCH!", m_warningColor);
        }
        private string Separator() 
        {
            return ColorText($" {m_separator.Trim()} ", m_separatorColor);
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