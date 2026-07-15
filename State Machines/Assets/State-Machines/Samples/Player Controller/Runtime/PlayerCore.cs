using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace M320.StateMachines.Samples.PlayerController
{
    public sealed class PlayerCore : StateMachineCore
    {
        #region Inspector Fields

        [Header("Components")]
        [SerializeField] private BoxCollider2D m_collider;
        [SerializeField] private Rigidbody2D m_rigidbody2D;

        [Header("Settings - Move")]
        [SerializeField, Range(0.1f, 20f)] private float m_moveSpeed = 5f;

        [Header("Settings - Jump")]
        [SerializeField, Range(0.1f, 20f)] private float m_jumpSpeed = 5f;

        [Header("Settings - Ground Check")]
        [SerializeField] private LayerMask m_groundLayer = 1;
        [SerializeField, Range(0.02f, 0.05f)] private float m_groundCheckDistance = 0.03f;
        [SerializeField, Range(0f, 0.1f)] private float m_groundCheckTolerance = 0.05f;

        #endregion

        #region Cache Fields & Properties

        private Keyboard _keyboard;
        private bool _isGrounded;
        private readonly RaycastHit2D[] _oneHitBuffer = new RaycastHit2D[1];

        #endregion

        private readonly GroundedState _groundedState = new("Grounded");
        private readonly AirboneState _airboneState = new("Airbone");

        protected override State InitialState => _groundedState;

        protected override object Context => this;

        protected override void Update()
        {
            base.Update();

            _keyboard = Keyboard.current;

            CheckIfGrounded();

            if (_isGrounded) ChangeState(_groundedState, true);
            else ChangeState(_airboneState, true);
        }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[] 
            {
                _groundedState,
                _airboneState
            };
        }

        private void CheckIfGrounded()
        {
            var filter = GroundFilter();
            int hitCount = m_collider.Cast(Vector2.down, filter, _oneHitBuffer, m_groundCheckDistance);
            _isGrounded = hitCount > 0 && _oneHitBuffer[0].normal.y >= 1f - m_groundCheckTolerance;
        }
        private ContactFilter2D GroundFilter()
        {
            return new() { layerMask = m_groundLayer };
        }

        public void TryMoveX() 
        {
            if (_keyboard == null) return;

            int move = (int)_keyboard.dKey.value - (int)_keyboard.aKey.value;
            m_rigidbody2D.linearVelocityX = move * m_moveSpeed;
        }
        public void TryJump()
        {
            if (_keyboard == null) return;

            if (_keyboard.spaceKey.wasPressedThisFrame)
            {
                m_rigidbody2D.linearVelocityY = m_jumpSpeed;
            }
        }
    }

    public sealed class GroundedState : State
    {
        private PlayerCore _playerCore;

        public GroundedState(string name = "") : base(name) { }

        public override void OnEnter(float startTime)
        {
            base.OnEnter(startTime);

            _playerCore = (PlayerCore)Context;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);

            _playerCore.TryJump();
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            _playerCore.TryMoveX();
        }
    }

    public sealed class AirboneState : State
    {
        private PlayerCore _playerCore;

        public AirboneState(string name = "") : base(name) { }

        public override void OnEnter(float startTime)
        {
            base.OnEnter(startTime);

            _playerCore = (PlayerCore)Context;
        }

        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);

            _playerCore.TryMoveX();
        }
    }
}
