using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public interface IStateIdentifier 
    {
        string Name { get; }
    }

    public sealed class StateDictionary<TState>
        where TState : IStateIdentifier
    {
        private readonly Dictionary<string, TState> _dictionary = new();

        public TState this[StateName name] => _dictionary[name];

        public IReadOnlyCollection<TState> States => _dictionary.Values;

        public void Add(TState state)
        {
            _dictionary.Add(state.Name, state);
        }
    }
}
