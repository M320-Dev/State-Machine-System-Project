using System;
using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public abstract class StateMachineCore : MonoBehaviour
    {
        [SerializeField] private string m_initialLeafState;

        protected readonly StateDictionary<State> StateDictionary = new();
        protected StateDictionary<StateDefinition> StateDefinitionDictionary { get; private set; }

        public event ChangeHandler<State> StateChanged;

        #region Properties

        protected abstract object ContextObject { get; }

        public IReadOnlyCollection<StateDefinition> Definitions => StateDefinitionDictionary.States;
        public IReadOnlyCollection<State> States => StateDictionary.States;

        protected StateMachine Machine { get; private set; }
        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        #endregion

        #region Get State/Definition

        public State GetState(StateName name)
        {
            return StateDictionary[name];
        }
        public StateDefinition GetStateDefinition(StateName name)
        {
            return StateDefinitionDictionary[name];
        }

        #endregion

        #region Initialization

        protected virtual void OnValidate() 
        {
            InitializeDefinitions();
        }

        protected virtual void Awake()
        {
            State initialState = InitializeInstances();
            Machine = new(initialState);
            Machine.StateChanged += (p, c) => StateChanged?.Invoke(p, c);
        }

        /// <returns>
        /// Initial State
        /// </returns>
        protected virtual State InitializeInstances() 
        {
            StateInitializer.SetupInstances(Definitions, ContextObject, StateDictionary);

            var (ancestryBranch, leaf) = GetInitialStateBranch();
            StateInitializer.SetupInitialStateBranch(ancestryBranch, leaf);

            return ancestryBranch.Count > 0 ? ancestryBranch[0] : leaf;
        }

        protected virtual void InitializeDefinitions()
        {
            if (StateDefinitionDictionary != null) return;

            StateDefinitionDictionary = new();

            StateInitializer.SetupDefinitions(DefineStates(), StateDefinitionDictionary);
        }
        protected abstract IEnumerable<StateDefinition> DefineStates();

        #endregion

        #region Update

        protected virtual void Update()
        {
            float deltaTime = Time.deltaTime;
            OnUpdate(deltaTime);
            Machine.Update(deltaTime);
        }
        protected virtual void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            OnFixedUpdate(fixedDeltaTime);
            Machine.FixedUpdate(Time.fixedDeltaTime);
        }

        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnFixedUpdate(float fixedDeltaTime) { }

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

        protected (List<HierarchicalState> ancestryBranch, State leaf) GetInitialStateBranch()
        {
            State foundState = null;

            foreach (var state in States)
            {
                Helper(state);
            }

            if (foundState == null)
            {
                throw new InvalidOperationException(
                    "Initial State Branch not found! " +
                    "The initial state must be a leaf and must exist!");
            }

            return (foundState.GetAncestryBranch(), foundState);

            void Helper(State state)
            {
                if (foundState != null) return;

                if (state is not HierarchicalState hierarchicalState)
                {
                    if (state.Name == m_initialLeafState) foundState = state;

                    return;
                }

                foreach (var _state in hierarchicalState.States)
                {
                    Helper(_state);
                }
            }
        }

        public List<State> GetCurrentStateBranch()
        {
            return Machine?.GetCurrentStateBranch();
        }
    }
    public abstract class StateMachineCore<TContext> : StateMachineCore
    {
        protected override object ContextObject => Context;
        protected abstract TContext Context { get; }
    }
    public abstract class ContextlessStateMachineCore : StateMachineCore<EmptyContext>
    {
        protected override EmptyContext Context => default;
    }
}
