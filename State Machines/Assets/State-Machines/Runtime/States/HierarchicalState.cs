using System.Collections.Generic;

namespace M320.StateMachines
{
    public abstract class HierarchicalState : State, IStateMachine
    {
        protected readonly StateDictionary StateDictionary = new();

        protected readonly StateMachine Machine;


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

        #region Constructors

        public HierarchicalState(StateName name, StateName initialStateName, float startTime = 0f) : base(name)
        {
            Initialize();
            InitializationUtility.Initialize(ConstructStates(), Context, StateDictionary);
            Machine = new(StateDictionary[initialStateName], startTime);
        }
        public HierarchicalState(StateName initialStateName, float startTime = 0f) 
            : this("", initialStateName, startTime) { }

        #endregion

        #region Initialization

        protected virtual void Initialize() { }
        protected abstract IReadOnlyList<State> ConstructStates();

        #endregion

        #region Properties

        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        #endregion

        #region Update

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

        #endregion

        #region Change State

        protected void ChangeState(StateName stateName, float startTime = 0f, bool forceChange = false)
        {
            ChangeState(StateDictionary[stateName], startTime, forceChange);
        }
        public void ChangeState(State state, float startTime = 0f, bool forceChange = false)
        {
            Machine.ChangeState(state, startTime, forceChange);
        }

        #endregion
    }
}
