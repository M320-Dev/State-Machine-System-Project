using System.Collections.Generic;

namespace M320.StateMachines
{
    public abstract class HierarchicalState : State, IStateMachine
    {
        protected readonly StateDictionary StateDictionary = new();

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
            Initialize();
            Machine = new(initialState, startTime);
        }

        protected virtual void Initialize()
        {
            foreach (var state in ConstructStates())
            {
                state.Context = Context;
                StateDictionary.Add(state);
            }
        }
        protected abstract IReadOnlyList<State> ConstructStates();

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
