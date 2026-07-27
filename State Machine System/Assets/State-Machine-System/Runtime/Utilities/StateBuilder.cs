using System;
using System.Collections.Generic;
using System.Linq;

namespace M320.StateMachineSystem
{
    public static class StateBuilder
    {
        public static State Build(StateDefinition definition)
        {
            return (State)Activator.CreateInstance(definition.type, definition, definition.parameters);
        }
    }
}
