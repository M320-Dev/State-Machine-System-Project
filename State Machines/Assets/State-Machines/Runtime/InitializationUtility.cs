using System.Collections.Generic;

namespace M320.StateMachines
{
    public static class InitializationUtility
    {
        public static void SetupStates(IReadOnlyList<State> states, object context, StateDictionary stateDictionary)
        {
            foreach (var state in states)
            {
                state.SetupContext(context);
                stateDictionary.Add(state);
            }
        }
    }
}
