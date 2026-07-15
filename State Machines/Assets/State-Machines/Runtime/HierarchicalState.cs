namespace M320.StateMachines
{
    public class HierarchicalState : State, IStateMachine
    {
        public readonly StateMachine Machine;

        public event ChangeHandler<State> StateChanged 
        {
            add 
            {
                if (Machine != null) Machine.StateChanged += value;
            }
            remove
            {
                if (Machine != null) Machine.StateChanged -= value;
            }
        }

        public HierarchicalState(State initialState, float startTime = 0f)
        {
            Machine = new(initialState, startTime);
        }

        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        public void ChangeState(State state, float startTime = 0f, bool forceChange = false)
        {
            Machine.ChangeState(state, startTime, forceChange);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            Machine.Update(deltaTime);
        }
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);
            Machine.FixedUpdate(fixedDeltaTime);
        }
    }
}
