using System;

namespace M320.StateMachineSystem
{
    public static class StateBuilder
    {
        public static State Build(StateDefinition definition)
        {
            return (State)Build_Internal(definition);
        }
        public static TState Build<TState>(StateDefinition<TState> definition)
            where TState : State
        {
            return (TState)Build_Internal(definition);
        }

        private static object Build_Internal(StateDefinition definition)
        {
            return Activator.CreateInstance(definition.type, definition);
        }
    }
}
