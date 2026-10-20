using UnityEditor;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public sealed class StateMachineCoreDebugger : StateMachineCoreDebuggerBase
    {
        private void OnDrawGizmos()
        {
#if UNITY_EDITOR

            if (!enabled) return;

            string currentStateBranchString = CurrentStateBranchString();
            GUIStyle style = Style();
            Handles.Label(transform.position, currentStateBranchString, style);

#endif
        }

#if UNITY_EDITOR

        private GUIStyle Style() 
        {
            GUIStyle style = GUI.skin.label;
            style.richText = true;
            style.alignment = TextAnchor.MiddleCenter;
            return style;
        }

#endif
    }
}