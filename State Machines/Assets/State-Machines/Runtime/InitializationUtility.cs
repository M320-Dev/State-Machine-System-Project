using System.Collections.Generic;

namespace M320.StateMachines
{
    public static class InitializationUtility
    {
        public static void Initialize(IReadOnlyList<State> states, object context, StateDictionary stateDictionary)
        {
            foreach (var state in states)
            {
                state.Context = context;
                stateDictionary.Add(state);
            }
        }
    }
}
