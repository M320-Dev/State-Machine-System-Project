namespace M320.StateMachines
{
    public abstract class StateMachineCore<TContext>
    {
        public readonly StateMachine<TContext> machine;

        public StateMachineCore()
        {
            machine = new();
        }
        public StateMachineCore(State<TContext> initialState) 
        {
            machine = new(initialState);
        }

        public void Update(float deltaTime) 
        {
            OnUpdate(deltaTime);

            machine.currentState?.OnUpdate(deltaTime);
        }
        public void FixedUpdate(float fixedDeltaTime)
        {
            OnFixedUpdate(fixedDeltaTime);

            machine.currentState?.OnFixedUpdate(fixedDeltaTime);
        }

        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnFixedUpdate(float fixedDeltaTime) { }
    }
}
