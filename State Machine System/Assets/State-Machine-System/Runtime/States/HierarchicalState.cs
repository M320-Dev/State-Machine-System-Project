using System.Collections.Generic;

namespace M320.StateMachineSystem
{
    public abstract class HierarchicalState : State
    {
        #region Fields

        new public readonly HierarchicalStateDefinition Definition;
        protected readonly StateDictionary<State> Children = new();
        protected readonly StateMachine Machine = new();

        #endregion

        public event ChangeHandler<State> StateChanged;

        public HierarchicalState(HierarchicalStateDefinition definition) : base(definition) 
        {
            Definition = definition;
            Machine.StateChanged += (p, c) => StateChanged?.Invoke(p, c);
        }

        #region Properties

        public IReadOnlyCollection<State> ChildStates => Children.States;
        public IReadOnlyCollection<StateDefinition> ChildDefinitions => Definition.Children;

        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        #endregion

        #region Initialization

        internal void SetupInstances() 
        {
            StateInitializer.SetupInstances(ChildDefinitions, Context, Children, this);
        }

        #endregion

        #region Set Initial State

        public void SetInitialState(StateName name)
        {
            SetInitialState(Children[name]);
        }
        public void SetInitialState(StateDefinition definition)
        {
            SetInitialState(Children[definition]);
        }
        public void SetInitialState(State initialState)
        {
            Machine.SetInitialState(initialState);
        }

        #endregion

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
            return ChangeState(Children[stateName], forceChange);
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
