using System;
using UnityEngine;

namespace M320.StateMachineSystem
{
    [Serializable]
    public sealed class InitialStateBranch
    {
        [SerializeField] private StateDefinition[] _children;
        [SerializeField] private int[] _selectedIndexes;

        public StateDefinition[] Children 
        {
            get => _children;
            internal set => _children = value;
        }
        public int[] SelectedIndexes
        {
            get => _selectedIndexes;
            set => _selectedIndexes = value;
        }
    }
}
