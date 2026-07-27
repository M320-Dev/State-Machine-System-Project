using System;

namespace M320.StateMachineSystem
{
    public class StateDefinition : IStateIdentifier
    {
        public readonly Type type;
        public readonly StateParameters parameters;

        public string Name => parameters.name;

        public HierarchicalStateDefinition Parent { get; internal set; }

        protected StateDefinition(Type type, StateParameters parameters) 
        {
            this.type = type;
            this.parameters = parameters;
        }

        public static StateDefinition Create<TState>(StateParameters parameters)
            where TState : State
        {
            return new(typeof(TState), parameters);
        }
    }
}
