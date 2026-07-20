using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public static class InitializationUtility
    {
        public static void SetupStates(IReadOnlyList<State> states, object context, StateDictionary stateDictionary, HierarchicalState parentState = null)
        {
            foreach (var state in states)
            {
                state.Parent = parentState;
                state.SetContext(context);
                stateDictionary.Add(state);

                if (state is not HierarchicalState hierarchicalState) continue;

                hierarchicalState.SetupStates();
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
