using System;
using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem
{
    public class State
    {
        public readonly string Name;

        #region Events

        public event Action Entered;
        public event Action Exited;

        #endregion

        #region Constructors

        public State(StateName name)
        {
            Name = name.IsNullOrWhiteSpace ? TypeName() : name;
        }
        public State()
        {
            Name = TypeName();
        }

        private string TypeName() 
        {
            return GetType().Name;
        }

        #endregion

        #region Properties

        public HierarchicalState Parent { get; internal set; }
        public object Context { get; internal set; }

        public virtual bool IsInitiallyFinished { get; } = true;

        public float StartTime { get; private set; }
        public float ElapsedTime { get; private set; }
        public float FixedElapsedTime { get; private set; }

        public float CurrentTime => StartTime + ElapsedTime;
        public float FixedCurrentTime => StartTime + FixedElapsedTime;

        public float GetElapsedTime(UpdateMode updateMode)
        {
            return updateMode switch
            {
                UpdateMode.Update => ElapsedTime,
                UpdateMode.FixedUpdate => FixedElapsedTime,
                _ => throw new ArgumentException("Invalid Update Mode!", nameof(updateMode))
            };
        }
        public float GetCurrentTime(UpdateMode updateMode)
        {
            return updateMode switch
            {
                UpdateMode.Update => CurrentTime,
                UpdateMode.FixedUpdate => FixedCurrentTime,
                _ => throw new ArgumentException("Invalid Update Mode!", nameof(updateMode))
            };
        }

        #endregion

        #region Methods

        internal virtual void SetContext(object context)
        {
            Context = context;
        }

        #region Enter

        protected virtual void OnReset() 
        {
            StartTime = Time.time;
            ElapsedTime = 0f;
            FixedElapsedTime = 0;
        }
        internal void Enter()
        {
            OnReset();

            OnEnter();

            Entered?.Invoke();
        }
        protected virtual void OnEnter() { }

        #endregion

        #region Update / Fixed Update

        internal virtual void Update(float deltaTime)
        {
            ElapsedTime += deltaTime;
            OnUpdate(deltaTime);
        }
        internal virtual void FixedUpdate(float fixedDeltaTime)
        {
            FixedElapsedTime += fixedDeltaTime;
            OnFixedUpdate(fixedDeltaTime);
        }

        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnFixedUpdate(float fixedDeltaTime) { }

        #endregion

        #region Exit

        internal void Exit() 
        {
            OnExit();

            Exited?.Invoke();
        }
        protected virtual void OnExit() { }

        #endregion

        #region Miscellaneous

        public List<State> GetBranch()
        {
            List<State> branch = new();
            for (State state = this; state != null; state = state.Parent)
            {
                branch.Add(state);
            }
            branch.Reverse();
            return branch;
        }
        public List<HierarchicalState> GetAncestryBranch()
        {
            List<HierarchicalState> branch = new();
            for (HierarchicalState state = Parent; state != null; state = state.Parent)
            {
                branch.Add(state);
            }
            branch.Reverse();
            return branch;
        }

        public bool IsNamed(Enum stateEnum)
        {
            return Name == stateEnum.ToString();
        }

        #endregion

        #endregion
    }
    public class State<TContext> : State
    {
        new protected TContext Context;

        public State(StateName name) : base(name) { }
        public State() : base() { }

        internal override void SetContext(object context)
        {
            base.SetContext(context);
            Context = (TContext)context;
        }
    }
}
