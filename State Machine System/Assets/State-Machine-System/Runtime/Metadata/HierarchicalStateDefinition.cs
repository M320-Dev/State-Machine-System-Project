using System;
using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public abstract class HierarchicalStateDefinition : StateDefinition
    {
        protected readonly StateDictionary<StateDefinition> StateDefinitionDictionary = new();

        protected HierarchicalStateDefinition(Type type, StateParameters parameters)
            : base(type, parameters) { }

        internal void SetupDefinitions()
        {
            StateInitialization.SetupDefinitions(DefineStates(), StateDefinitionDictionary, this);
        }
        protected abstract IEnumerable<StateDefinition> DefineStates();
    }
}
