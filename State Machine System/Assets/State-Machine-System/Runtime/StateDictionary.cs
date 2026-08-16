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
        private readonly List<TState> _states = new();
        private readonly Dictionary<string, TState> _nameDictionary = new();

        public IReadOnlyList<TState> States => _states;
        
        public TState this[int index] => _states[index];
        public TState this[StateName name] => _nameDictionary[name];

        public void Add(TState state)
        {
            _states.Add(state);
            _nameDictionary.Add(state.Name, state);
        }
    }
}
