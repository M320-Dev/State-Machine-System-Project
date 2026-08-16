using System.Collections.Generic;

namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class WanderStateDefinition : HierarchicalStateDefinition<WanderState, AICore>
    {
        public WanderStateDefinition(StateParameters parameters) : base(parameters) { }

        protected override IEnumerable<StateDefinition> DefineChildren()
        {
            return new StateDefinition[]
            {
                new TimedStateDefinition<StopMovingXState>(
                    new TimedStateParameters(WanderState.StateEnum.Idle, Context.RandomIdleDuration())
                    ),

                new TimedStateDefinition<MoveState>(
                    new TimedStateParameters(WanderState.StateEnum.Move, Context.RandomMoveDuration())
                    )
            };
        }
    }
}
