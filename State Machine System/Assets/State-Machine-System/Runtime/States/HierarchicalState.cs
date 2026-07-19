using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public abstract class HierarchicalState : State, IStateMachine
    {
        protected readonly StateDictionary StateDictionary = new();

        protected readonly StateMachine Machine = new();

        public event ChangeHandler<State> StateChanged;

        #region Constructors

        public HierarchicalState(StateName name) : base(name)
        {
            InitializationUtility.SetupStates(ConstructStates(), Context, StateDictionary, this);
            Machine.StateChanged += (p, c) => StateChanged?.Invoke(p, c);
        }
        public HierarchicalState() 
            : this("") { }

        #endregion

        #region Properties

        public IReadOnlyCollection<State> States => StateDictionary.States;

        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        #endregion

        #region Initialization

        protected abstract IReadOnlyList<State> ConstructStates();

        #endregion

        #region Set Initial State

        public void SetInitialState(State initialState)
        {
            Machine.SetInitialState(initialState);
        }

        #endregion

        #region Update

        internal override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            Machine.Update(deltaTime);
        }
        internal override void FixedUpdate(float fixedDeltaTime)
        {
            base.FixedUpdate(fixedDeltaTime);
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

        #region Constructors

        public HierarchicalState(StateName name) : base(name) { }
        public HierarchicalState() : base() { }

        #endregion

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
