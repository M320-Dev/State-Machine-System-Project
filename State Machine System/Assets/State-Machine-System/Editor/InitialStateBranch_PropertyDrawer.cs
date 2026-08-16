using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace M320.StateMachineSystem.Editor
{
    [CustomPropertyDrawer(typeof(InitialStateBranch))]
    public sealed class InitialStateBranch_PropertyDrawer : PropertyDrawer
    {
        private static readonly float LineHeight = EditorGUIUtility.singleLineHeight;
        private static readonly float LineSpacing = EditorGUIUtility.standardVerticalSpacing;
        private static readonly float LineDeltaY = LineHeight + LineSpacing;

        private class Entry
        {
            public readonly StateDefinition definition;
            public readonly IReadOnlyCollection<StateDefinition> definitionOptions;

            private int _selectedIndex;

            public readonly GUIContent label;
            public readonly GUIContent[] options;

            public StateDefinition SelectedDefinition { get; private set; }

            public Entry(HierarchicalStateDefinition hierarchicalDefinition, int selectedIndex) 
            {
                definition = hierarchicalDefinition;
                definitionOptions = hierarchicalDefinition.Children;
                label = new(definition.Name);
                _selectedIndex = selectedIndex;
                options = ToOptions(definitionOptions, selectedIndex, out StateDefinition selectedDefinition);
                SelectedDefinition = selectedDefinition;
            }
            public Entry(IReadOnlyCollection<StateDefinition> definitionOptions, int selectedIndex, GUIContent label)
            {
                this.definitionOptions = definitionOptions;
                this.label = label;
                _selectedIndex = selectedIndex;
                options = ToOptions(definitionOptions, selectedIndex, out StateDefinition selectedDefinition);
                SelectedDefinition = selectedDefinition;
            }

            public int SelectedIndex 
            {
                get => _selectedIndex;
                set => SetSelectedIndex(value);
            }

            private void SetSelectedIndex(int selectedIndex) 
            {
                if (_selectedIndex != selectedIndex)
                {
                    _selectedIndex = selectedIndex;

                    if (definitionOptions == null) return;

                    SelectedDefinition = GetSelectedDefinition(definitionOptions, selectedIndex);
                }
            }
        }

        private InitialStateBranch _initialStateBranch;

        private Entry[] _entries;

        private bool _initialized;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _initialStateBranch ??= Utility.GetLeafTargetObject<InitialStateBranch>(property);

            TryInitialize(label);

            EditorGUI.BeginProperty(position, label, property);

            EntryPopups(position);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int length = _entries != null ? _entries.Length : 0;
            return LineDeltaY * length;
        }

        private void EntryPopups(Rect position) 
        {
            bool selectedIndexChanged = false;
            int depthAtChange = 0;

            for (int depth = 0; depth < _entries.Length; depth++)
            {
                var entry = _entries[depth];

                Rect entryPosition = new(position.x, position.y + LineDeltaY * depth, position.width, LineHeight);

                int previousSelectedIndex = entry.SelectedIndex;
                entry.SelectedIndex = EditorGUI.Popup(entryPosition, entry.label, entry.SelectedIndex, entry.options);

                if (previousSelectedIndex != entry.SelectedIndex) 
                {
                    selectedIndexChanged = true;
                    depthAtChange = depth;
                }
            }

            if (selectedIndexChanged) OnSelectedIndexChanged(depthAtChange);
        }

        private void OnSelectedIndexChanged(int depth) 
        {
            Update(depth);
            UpdatePerservedSelection();
        }

        #region Perservation

        private void UpdatePerservedSelection()
        {
            _initialStateBranch.Selection = new(_entries.Length);

            for (int i = 0; i < _entries.Length; i++)
            {
                var entry = _entries[i];
                _initialStateBranch.Selection.Set(i, entry.SelectedDefinition, entry.SelectedIndex);
            }
        }

        private int GetPreservedIndexOrZero(int index, IReadOnlyCollection<StateDefinition> definitionOptions)
        {
            if (!TryGetPreservedIndex(index, out int selectedIndex)) return 0;

            return selectedIndex >= 0 && selectedIndex < definitionOptions.Count ? selectedIndex : 0;
        }
        private bool TryGetPreservedIndex(int index, out int selectedIndex)
        {
            selectedIndex = -1;
            if (_initialStateBranch.Selection == null) return false;
            if (index < 0 || index >= _initialStateBranch.Selection.Length) return false;

            selectedIndex = _initialStateBranch.Selection.Indexes[index];
            return true;
        }

        #endregion

        private void Update(int depth) 
        {
            var entry = _entries[depth];

            List<Entry> entriesRight = new();

            AddEntriesRecursively(entriesRight, entry.SelectedDefinition);

            _entries = CombineEntries(depth, entriesRight);
        }

        private Entry[] CombineEntries(int depth, List<Entry> entriesRight) 
        {
            int length = depth + 1;
            int totalLength = length + entriesRight.Count;
            var entries = new Entry[totalLength];

            for (int i = 0; i < length; i++)
            {
                entries[i] = _entries[i];
            }
            for (int i = length; i < totalLength; i++)
            {
                entries[i] = entriesRight[i - length];
            }

            return entries;
        }

        private void TryInitialize(GUIContent label)
        {
            if (_initialized) return;

            Reinitialize(label);

            _initialized = true;
        }

        private void Reinitialize(GUIContent label)
        {
            List<Entry> entries = new();

            int selectedIndex = GetPreservedIndexOrZero(0, _initialStateBranch.Children);
            Entry entry = new(_initialStateBranch.Children, selectedIndex, label);

            entries.Add(entry);

            AddEntriesRecursively(entries, entry.SelectedDefinition, 1);

            _entries = entries.ToArray();
        }

        private void AddEntriesRecursively(List<Entry> entries, StateDefinition definition, int depth) 
        {
            if (definition is not HierarchicalStateDefinition hierarchicalDefinition) return;

            int selectedIndex = GetPreservedIndexOrZero(depth, hierarchicalDefinition.Children);
            Entry entry = new(hierarchicalDefinition, selectedIndex);

            entries.Add(entry);

            AddEntriesRecursively(entries, entry.SelectedDefinition, depth + 1);
        }
        private void AddEntriesRecursively(List<Entry> entries, StateDefinition definition)
        {
            if (definition is not HierarchicalStateDefinition hierarchicalDefinition) return;

            Entry entry = new(hierarchicalDefinition, 0);

            entries.Add(entry);

            AddEntriesRecursively(entries, entry.SelectedDefinition);
        }

        #region To Options

        private static GUIContent[] ToOptions(HierarchicalStateDefinition definition) 
        {
            return ToOptions(definition.Children);
        }
        private static GUIContent[] ToOptions(HierarchicalStateDefinition definition, int selectedIndex, out StateDefinition selectedDefinition)
        {
            return ToOptions(definition.Children, selectedIndex, out selectedDefinition);
        }

        private static GUIContent[] ToOptions(IReadOnlyCollection<StateDefinition> definitions)
        {
            var enumerator = definitions.GetEnumerator();

            GUIContent[] options = new GUIContent[definitions.Count];

            for (int i = 0; i < definitions.Count; i++)
            {
                enumerator.MoveNext();
                var child = enumerator.Current;
                options[i] = new(child.Name);
            }

            return options;
        }
        private static GUIContent[] ToOptions(IReadOnlyCollection<StateDefinition> definitions, int selectedIndex, out StateDefinition selectedDefinition)
        {
            selectedDefinition = null;

            var enumerator = definitions.GetEnumerator();

            GUIContent[] options = new GUIContent[definitions.Count];

            for (int i = 0; i < definitions.Count; i++)
            {
                enumerator.MoveNext();
                var child = enumerator.Current;
                options[i] = new(child.Name);

                if (i == selectedIndex) selectedDefinition = child;
            }

            return options;
        }

        #endregion

        #region Get Selected Definition

        private static StateDefinition GetSelectedDefinition(HierarchicalStateDefinition definition, int selectedIndex)
        {
            return GetSelectedDefinition(definition.Children, selectedIndex);
        }
        private static StateDefinition GetSelectedDefinition(IReadOnlyCollection<StateDefinition> definitions, int selectedIndex) 
        {
            var enumerator = definitions.GetEnumerator();

            for (int i = 0; i < definitions.Count; i++)
            {
                enumerator.MoveNext();
                var child = enumerator.Current;

                if (i == selectedIndex) return child;
            }

            return null;
        }

        #endregion
    }
}
