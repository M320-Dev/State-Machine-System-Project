using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public interface IStateIdentifier
    {
        string Name { get; }
        StateDefinition Definition { get; }
    }

    public sealed class StateDictionary<TState>
        where TState : IStateIdentifier
    {
        private readonly Dictionary<string, TState> _nameDictionary = new();
        private readonly Dictionary<StateDefinition, TState> _definitionDictionary = new();

        public TState this[StateName name] => _nameDictionary[name];
        public TState this[StateDefinition definition] => _definitionDictionary[definition];

        public IReadOnlyCollection<TState> States => _nameDictionary.Values;

        public void Add(TState state)
        {
            _nameDictionary.Add(state.Name, state);
            _definitionDictionary.Add(state.Definition, state);
        }
    }
}
