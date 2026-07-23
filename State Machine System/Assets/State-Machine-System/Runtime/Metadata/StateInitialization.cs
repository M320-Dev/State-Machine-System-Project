using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public static class StateInitialization
    {
        public static void SetupRelations(IEnumerable<StateDefinition> definitions, HierarchicalStateDefinition parentState = null)
        {
            foreach (var definition in definitions)
            {
                definition.Parent = parentState;

                if (definition is not HierarchicalStateDefinition hierarchicalState) continue;

                SetupRelations(hierarchicalState.DefineStates(), hierarchicalState);
            }
        }
    }
}
