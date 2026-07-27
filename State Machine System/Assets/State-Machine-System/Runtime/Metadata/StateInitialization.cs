using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public static class StateInitialization
    {
        public static void SetupDefinitions(IEnumerable<StateDefinition> definitions, StateDictionary<StateDefinition> definitionDictionary, HierarchicalStateDefinition parentState = null)
        {
            foreach (var definition in definitions)
            {
                definition.Parent = parentState;
                definitionDictionary.Add(definition);

                if (definition is not HierarchicalStateDefinition hierarchicalState) continue;

                hierarchicalState.SetupDefinitions();
            }
        }
    }
}
