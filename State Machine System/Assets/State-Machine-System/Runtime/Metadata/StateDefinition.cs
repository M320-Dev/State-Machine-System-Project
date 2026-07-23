using System;

namespace M320.StateMachineSystem
{
    public class StateDefinition
    {
        public readonly Type type;
        public readonly StateParameters parameters;

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
