using System;
using UnityEngine;

namespace M320.StateMachineSystem
{
    [Serializable]
    public sealed class InitialStateBranch
    {
        [Serializable]
        public sealed class SelectedDefinitionsAndIndexes 
        {
            [SerializeField] private StateDefinition[] _definitions;
            [SerializeField] private int[] _indexes;
            [SerializeField] private int _length;

            public SelectedDefinitionsAndIndexes(int length)
            {
                _definitions = new StateDefinition[length];
                _indexes = new int[length];
                _length = length;
            }

            public StateDefinition[] Definitions => _definitions;
            public int[] Indexes => _indexes;
            public int Length => _length;

            public void Set(int index, StateDefinition selectedDefinition, int selectedIndex)
            {
                Definitions[index] = selectedDefinition;
                Indexes[index] = selectedIndex;
            }
        }

        [SerializeField] private StateDefinition[] _children;
        [SerializeField] private SelectedDefinitionsAndIndexes _selection;

        public StateDefinition[] Children 
        {
            get => _children;
            internal set => _children = value;
        }
        public SelectedDefinitionsAndIndexes Selection 
        {
            get => _selection;
            set => _selection = value;
        }
    }
}
