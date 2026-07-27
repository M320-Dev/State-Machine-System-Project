using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public abstract class HierarchicalState : State
    {
        #region Fields

        new public readonly HierarchicalStateDefinition Definition;
        protected readonly StateDictionary<State> StateDictionary = new();
        protected readonly StateMachine Machine = new();

        #endregion

        public event ChangeHandler<State> StateChanged;

        public HierarchicalState(HierarchicalStateDefinition definition) : base(definition) 
        {
            Definition = definition;
            Machine.StateChanged += (p, c) => StateChanged?.Invoke(p, c);
        }

        #region Properties

        public IReadOnlyCollection<State> States => StateDictionary.States;
        public IReadOnlyCollection<StateDefinition> Definitions => Definition.Definitions;

        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        #endregion

        #region Initialization

        internal void SetupInstances() 
        {
            StateInitializer.SetupInstances(Definition.Definitions, Context, StateDictionary, this);
        }

        #endregion

        public void SetInitialState(State initialState)
        {
            Machine.SetInitialState(initialState);
        }

        public List<State> GetCurrentStateBranch() 
        {
            return Machine.GetCurrentStateBranch();
        }

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

        public bool ChangeState(StateName stateName, bool forceChange = false)
        {
            return ChangeState(StateDictionary[stateName], forceChange);
        }
        public bool ChangeState(State state, bool forceChange = false)
        {
            return Machine.ChangeState(state, forceChange);
        }

        #endregion
    }
    public abstract class HierarchicalState<TContext> : HierarchicalState
    {
        new protected TContext Context;

        public HierarchicalState(HierarchicalStateDefinition definition) : base(definition) { }

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
