using System;

namespace M320.StateMachineSystem
{
    [Serializable]
    public class StateDefinition : IStateIdentifier
    {
        public readonly Type type;
        public readonly StateParameters parameters;
        public string Name => parameters.name;

        public HierarchicalStateDefinition Parent { get; internal set; }

        #region Context

        private object _context;
        public object Context => _context;
        internal virtual void SetContext(object context)
        {
            _context = context;
        }

        #endregion

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
    public class StateDefinition<TState> : StateDefinition
        where TState : State
    {
        public StateDefinition(StateParameters parameters) : base(typeof(TState), parameters) { }
    }
    public class StateDefinition<TState, TContext> : StateDefinition
            where TState : State
    {
        new public TContext Context { get; private set; }

        public StateDefinition(StateParameters parameters) : base(typeof(TState), parameters) { }

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
