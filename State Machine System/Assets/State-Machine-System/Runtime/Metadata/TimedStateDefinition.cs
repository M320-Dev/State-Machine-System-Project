using System;

namespace M320.StateMachineSystem
{
    public class TimedStateDefinition : StateDefinition
    {
        new public readonly TimedStateParameters parameters;

        protected TimedStateDefinition(Type type, TimedStateParameters parameters) : base(type, parameters) 
        {
            this.parameters = parameters;
        }

        public static TimedStateDefinition Create<TState>(TimedStateParameters parameters)
            where TState : State, ITimed
        {
            return new(typeof(TState), parameters);
        }
    }
    public class TimedStateDefinition<TState> : TimedStateDefinition
            where TState : State
    {
        public TimedStateDefinition(TimedStateParameters parameters) : base(typeof(TState), parameters) { }
    }
    public class TimedStateDefinition<TState, TContext> : TimedStateDefinition
        where TState : State
    {
        new public TContext Context { get; private set; }

        public TimedStateDefinition(TimedStateParameters parameters) : base(typeof(TState), parameters) { }

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
