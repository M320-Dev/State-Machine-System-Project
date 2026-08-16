using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public abstract class StateMachineCore : MonoBehaviour
    {
        [SerializeField] private InitialStateBranch m_initialStateBranch;

/*        [Space(16)]

        [SerializeField] private string m_initialLeafState;*/

        protected readonly StateDictionary<State> Children = new();
        protected StateDictionary<StateDefinition> ChildDefinitions { get; private set; }

        public event ChangeHandler<State> StateChanged;

        #region Properties

        protected abstract object ContextObject { get; }

        public IReadOnlyCollection<StateDefinition> ReadOnlyChildDefinitions => ChildDefinitions.States;

        protected StateMachine Machine { get; private set; }
        public State PreviousState => Machine?.PreviousState;
        public State CurrentState => Machine?.CurrentState;

        #endregion

        #region Get Child/Definition

        public State GetChild(StateName name) => Children[name];
        public StateDefinition GetChildDefinition(StateName name) => ChildDefinitions[name];

        #endregion

        #region Initialization

        protected virtual void OnValidate() 
        {
            InitializeDefinitions();
            DebugLogInitialBranchSelection();
        }

        protected virtual void Awake()
        {
            InitializeDefinitions();
            DebugLogInitialBranchSelection();

            InitializeInstances();

            var definitions = m_initialStateBranch.Selection.Definitions;
            State initialState = Children[definitions[0]];

            Machine = new(initialState);
            Machine.StateChanged += (p, c) => StateChanged?.Invoke(p, c);
        }

        private void DebugLogInitialBranchSelection()
        {
            Debug.Log($"===== Initial Branch Selection =====");

            for (int i = 0; i < m_initialStateBranch.Selection.Length; i++)
            {
                Debug.Log($"{m_initialStateBranch.Selection.Definitions[i].Name} | {m_initialStateBranch.Selection.Indexes[i]}");
            }

            Debug.Log($"====================================");
        }
        private void DebugLogChildren()
        {
            Debug.Log($"===== Children =====");

            foreach (var state in Children.States)
            {
                Debug.Log(state.Name);
            }

            Debug.Log($"====================");
        }

        protected virtual void InitializeDefinitions()
        {
            if (ChildDefinitions != null) return;

            ChildDefinitions = new();

            StateInitializer.SetupChildren(DefineChildren(), ContextObject, ChildDefinitions);

            m_initialStateBranch.Children = ChildrenArray();
        }
        protected abstract IEnumerable<StateDefinition> DefineChildren();
        private StateDefinition[] ChildrenArray()
        {
            var states = ChildDefinitions.States;
            var statesEnumerator = states.GetEnumerator();
            var definitions = new StateDefinition[states.Count];
            int i = 0;

            while (statesEnumerator.MoveNext()) 
            {
                definitions[i] = statesEnumerator.Current;
                i++;
            }

            return definitions;
        }

        /// <returns>
        /// Initial State
        /// </returns>
        protected virtual void InitializeInstances() 
        {
            StateInitializer.SetupInstances(ChildDefinitions.States, ContextObject, Children);
            DebugLogChildren();
            StateInitializer.SetupInitialStateBranch(m_initialStateBranch, Children);

            //var (ancestryBranch, leaf) = GetInitialStateBranch();
            //StateInitializer.SetupInitialStateBranch(ancestryBranch, leaf);

            //return ancestryBranch.Count > 0 ? ancestryBranch[0] : leaf;
        }

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
            return ChangeState(Children[stateName], forceChange);
        }
        public bool ChangeState(State state, bool forceChange = false)
        {
            return Machine.ChangeState(state, forceChange);
        }

        #endregion

        /*protected (List<HierarchicalState> ancestryBranch, State leaf) GetInitialStateBranch()
        {
            State foundState = null;

            foreach (var state in Children.States)
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
        }*/

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
