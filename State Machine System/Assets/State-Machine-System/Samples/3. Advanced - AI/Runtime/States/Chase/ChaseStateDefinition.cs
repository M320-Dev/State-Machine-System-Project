using System.Collections.Generic;

namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class ChaseStateDefinition : HierarchicalStateDefinition<ChaseState, AICore>
    {
        public ChaseStateDefinition(StateParameters parameters) : base(parameters) { }

        protected override IEnumerable<StateDefinition> DefineChildren()
        {
            return new StateDefinition[]
            {
                new TimedStateDefinition<StopMovingXState>(
                    new TimedStateParameters(ChaseState.StateEnum.Alerted, Context.alerted)
                    ),

                new StateDefinition<PursueState>(
                    new StateParameters(ChaseState.StateEnum.Pursue)
                    ),

                new TimedStateDefinition<StopMovingXState>(
                    new TimedStateParameters(ChaseState.StateEnum.Failed, Context.failed)
                    ),

                 new TimedStateDefinition<StopMovingXState>(
                    new TimedStateParameters(ChaseState.StateEnum.Succeeded, Context.succeeded)
                    ),
            };
        }
    }
}
