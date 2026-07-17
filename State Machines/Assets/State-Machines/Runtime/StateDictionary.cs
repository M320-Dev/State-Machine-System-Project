using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public sealed class StateDictionary
    {
        private readonly Dictionary<string, State> _dictionary = new();

        public State this[StateName name] => _dictionary[name];

        public IReadOnlyCollection<State> States => _dictionary.Values;

        public void Add(State state)
        {
            _dictionary.Add(state.Name, state);
        }
    }
}
