using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public abstract class StateMachineCoreDebuggerBase : MonoBehaviour
    {
        #region Inspector Fields

        [Header("Component")]
        [SerializeField] private StateMachineCoreBase m_core;

        [Header("Style Settings")]
        [SerializeField] private string m_separator = "=>";
        [SerializeField] private Color m_separatorColor = Color.yellow;
        [SerializeField] private Color m_stateNameColor = Color.white;
        [SerializeField] private Color m_errorColor = Color.red;

        #endregion

        protected string CurrentStateBranchString() 
        {
            if (m_core == null) return NoStateMachineCore();

            List<State> currentStateBranch = m_core.GetCurrentStateBranch();
            if (currentStateBranch == null || currentStateBranch.Count == 0) return NoCurrentStateBranch();

            string currentStateBranchString = StateName(currentStateBranch[0]);
            string seperator = Separator();

            for (int i = 1; i < currentStateBranch.Count; i++)
            {
                string stateName = StateName(currentStateBranch[i]);
                currentStateBranchString += seperator + stateName;
            }

            return currentStateBranchString;
        }

        #region Helper Methods

        private string NoStateMachineCore()
        {
            return ColorText("NO STATE MACHINE CORE!", m_errorColor);
        }
        private string NoCurrentStateBranch()
        {
            return ColorText("NO CURRENT STATE BRANCH!", m_errorColor);
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