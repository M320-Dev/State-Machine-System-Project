using System;
using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    [Serializable]
    public abstract class HierarchicalStateDefinition : StateDefinition
    {
        private readonly StateDictionary<StateDefinition> _children = new();

        public IReadOnlyCollection<StateDefinition> Children => _children.States;

        protected HierarchicalStateDefinition(Type type, StateParameters parameters) : base(type, parameters) { }

        internal void SetupChildren()
        {
            StateInitializer.SetupChildren(DefineChildren(), Context, _children, this);
        }
        protected abstract IEnumerable<StateDefinition> DefineChildren();
    }
    public abstract class HierarchicalStateDefinition<TState> : HierarchicalStateDefinition
    where TState : State
    {
        protected HierarchicalStateDefinition(StateParameters parameters) : base(typeof(TState), parameters) { }
    }
    public abstract class HierarchicalStateDefinition<TState, TContext> : HierarchicalStateDefinition
        where TState : State
    {
        new public TContext Context { get; private set; }

        protected HierarchicalStateDefinition(StateParameters parameters) : base(typeof(TState), parameters) { }

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
