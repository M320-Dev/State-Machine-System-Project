using System;
using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachines
{
    public class State
    {
        #region Properties

        public string Name { get; }

        public HierarchicalState Parent { get; internal set; }
        public object Context { get; internal set; }

        public virtual bool IsInitiallyFinished { get; } = true;
        public bool IsFinished { get; protected set; }

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

        #region Constructors

        public State(StateName name)
        {
            Name = string.IsNullOrWhiteSpace(name) ? TypeName() : name;
        }
        public State()
        {
            Name = TypeName();
        }

        private string TypeName() => GetType().Name;

        #endregion

        #region Methods

        public virtual void SetupContext(object context)
        {
            Context = context;
        }

        public virtual void OnEnter()
        {
            IsFinished = IsInitiallyFinished;
            StartTime = Time.time;
            ElapsedTime = 0f;
            FixedElapsedTime = 0;
        }

        public virtual void OnUpdate(float deltaTime)
        {
            ElapsedTime += deltaTime;
        }
        public virtual void OnFixedUpdate(float fixedDeltaTime)
        {
            FixedElapsedTime += fixedDeltaTime;
        }

        public virtual void OnExit() { }

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
    }
    public class State<TContext> : State
    {
        new protected TContext Context;

        public State(StateName name) : base(name) { }
        public State() : base() { }

        public override void SetupContext(object context)
        {
            base.SetupContext(context);
            Context = (TContext)context;
        }
    }
}
