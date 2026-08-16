using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    internal static class StateInitializer
    {
        internal static void SetupChildren(
            IEnumerable<StateDefinition> definitions,
            object context,
            StateDictionary<StateDefinition> definitionDictionary,
            HierarchicalStateDefinition parent = null)
        {
            foreach (var definition in definitions)
            {
                definition.Parent = parent;
                definition.SetContext(context);
                definitionDictionary.Add(definition);

                if (definition is not HierarchicalStateDefinition hierarchicalDefinition) continue;

                hierarchicalDefinition.SetupChildren();
            }
        }

        internal static void SetupInstances(
            IEnumerable<StateDefinition> definitions, 
            object context, 
            StateDictionary<State> stateDictionary, 
            HierarchicalState parent = null)
        {
            foreach (var definition in definitions)
            {
                State state = StateBuilder.Build(definition);

                state.Parent = parent;
                state.SetContext(context);
                stateDictionary.Add(state);

                if (state is not HierarchicalState hierarchicalState) continue;

                hierarchicalState.SetupInstances();
            }
        }

/*        internal static void SetupInitialStateBranch(List<HierarchicalState> ancestryBranch, State leaf)
        {
            if (ancestryBranch.Count <= 0) return;

            var ancestor = ancestryBranch[0];
            for (int i = 1; i < ancestryBranch.Count; i++)
            {
                ancestor.SetInitialState(ancestryBranch[i]);
                ancestor = ancestryBranch[i];
            }
            ancestor.SetInitialState(leaf);
        }*/
        internal static void SetupInitialStateBranch(InitialStateBranch initialStateBranch, StateDictionary<State> Children)
        {
            var definitions = initialStateBranch.Selection.Definitions;
            HierarchicalState ancestor = (HierarchicalState)Children[definitions[0]];
            int length = definitions.Length - 1;

            for (int i = 1; i < length; i++)
            {
                var current = (HierarchicalState)Children[definitions[i]];
                ancestor.SetInitialState(current);
                ancestor = current;
            }

            State leaf = Children[definitions[^1]];
            ancestor.SetInitialState(leaf);
        }
    }
}
