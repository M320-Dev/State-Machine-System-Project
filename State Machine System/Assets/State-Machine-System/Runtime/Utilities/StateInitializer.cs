using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public static class StateInitializer
    {
        public static void SetupDefinitions(
            IEnumerable<StateDefinition> definitions, 
            StateDictionary<StateDefinition> definitionDictionary, 
            HierarchicalStateDefinition parentState = null)
        {
            foreach (var definition in definitions)
            {
                definition.Parent = parentState;
                definitionDictionary.Add(definition);

                if (definition is not HierarchicalStateDefinition hierarchicalState) continue;

                hierarchicalState.SetupDefinitions();
            }
        }

        public static void SetupInstances(
            IEnumerable<StateDefinition> definitions, 
            object context, 
            StateDictionary<State> stateDictionary, 
            HierarchicalState parentState = null)
        {
            foreach (var definition in definitions)
            {
                State state = StateBuilder.Build(definition);

                state.Parent = parentState;
                state.SetContext(context);
                stateDictionary.Add(state);

                if (state is not HierarchicalState hierarchicalState) continue;

                hierarchicalState.SetupInstances();
            }
        }

        public static void SetupInitialStateBranch(List<HierarchicalState> ancestryBranch, State leaf)
        {
            if (ancestryBranch.Count <= 0) return;

            var ancestor = ancestryBranch[0];
            for (int i = 1; i < ancestryBranch.Count; i++)
            {
                ancestor.SetInitialState(ancestryBranch[i]);
                ancestor = ancestryBranch[i];
            }
            ancestor.SetInitialState(leaf);
        }
    }
}
