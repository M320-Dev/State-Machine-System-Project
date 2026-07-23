using System;
using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public abstract class HierarchicalStateDefinition : StateDefinition
    {
        protected HierarchicalStateDefinition(Type type, StateParameters parameters)
            : base(type, parameters) { }

        public abstract IEnumerable<StateDefinition> DefineStates();
    }
}
