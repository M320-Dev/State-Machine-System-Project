using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachines
{
    public sealed class StateMachineCoreDebugger : MonoBehaviour
    {
        [SerializeField] private StateMachineCore m_core;

        private void Update()
        {
            List<State> currentStateBranch = m_core.GetCurrentStateBranch();
            string s = currentStateBranch[0].GetType().Name;

            for (int i = 1; i < currentStateBranch.Count; i++)
            {
                s += " => " + currentStateBranch[i].GetType().Name;
            }

            Debug.Log(s);
        }
    }
}
