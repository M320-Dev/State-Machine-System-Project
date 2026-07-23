using System;

namespace M320.StateMachineSystem
{
    public static class StateBuilder
    {
        public static State Build(StateDefinition definition)
        {
            return (State)Activator.CreateInstance(
                definition.type, 
                definition.parameters);
        }
    }
}
