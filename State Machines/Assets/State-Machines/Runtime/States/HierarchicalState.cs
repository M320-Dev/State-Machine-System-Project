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

        public HierarchicalState(StateName name, StateName initialStateName) : base(name)
        {
            InitializationUtility.SetupStates(ConstructStates(), Context, StateDictionary);
            Machine = new(StateDictionary[initialStateName]);
        }
        public HierarchicalState(StateName initialStateName) 
            : this("", initialStateName) { }

        #endregion

        #region Initialization

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

        protected void ChangeState(StateName stateName, bool forceChange = false)
        {
            ChangeState(StateDictionary[stateName], forceChange);
        }
        public void ChangeState(State state, bool forceChange = false)
        {
            Machine.ChangeState(state, forceChange);
        }

        #endregion
    }
    public abstract class HierarchicalState<TContext> : HierarchicalState
    {
        new protected TContext Context;

        public HierarchicalState(StateName name, StateName initialStateName) 
            : base(name, initialStateName) { }
        public HierarchicalState(StateName initialStateName)
            : base(initialStateName) { }

        public override void SetupContext(object context)
        {
            base.SetupContext(context);
            Context = (TContext)context;
        }
    }
}
