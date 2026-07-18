using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace M320.StateMachineSystem.Samples.PlayerController
{
    public sealed class PlayerCore : StateMachineCore<PlayerCore>
    {
        public enum StateEnum
        {
            Grounded,
            Airbone
        }

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

        new public BoxCollider2D collider => m_collider;
        new public Rigidbody2D rigidbody2D => m_rigidbody2D;

        private Keyboard _keyboard;

        private bool _isGrounded;

        private readonly RaycastHit2D[] _oneHitBuffer = new RaycastHit2D[1];

        #endregion

        protected override PlayerCore Context => this;

        protected override void OnUpdate(float deltaTime)
        {
            _keyboard = Keyboard.current;

            CheckIfGrounded();

            if (_isGrounded) ChangeState(StateEnum.Grounded);
            else ChangeState(StateEnum.Airbone);
        }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[] 
            {
                new GroundedState(StateEnum.Grounded),
                new AirboneState(StateEnum.Airbone)
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
}
