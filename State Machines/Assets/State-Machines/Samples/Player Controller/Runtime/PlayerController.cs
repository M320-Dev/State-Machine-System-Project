using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace M320.StateMachines.Samples.PlayerController
{
    public class PlayerController : MonoBehaviour
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

        #region Events

        public event Action Jumped;
        public event Action Landed;

        #endregion

        private void Update()
        {
            _keyboard = Keyboard.current;

            CheckIfGrounded();

            if (_keyboard != null) OnKeyboardUpdate();
            else 
            {
                m_rigidbody2D.linearVelocityX = 0f;
            }
        }

        private void CheckIfGrounded()
        {
            var filter = GroundFilter();
            int hitCount = m_collider.Cast(Vector2.down, filter, _oneHitBuffer, m_groundCheckDistance);
            bool isGrounded = hitCount > 0 && _oneHitBuffer[0].normal.y >= 1f - m_groundCheckTolerance;

            if (!_isGrounded && isGrounded) 
            {
                Landed?.Invoke();
            }
            else if (_isGrounded && !isGrounded)
            {
                Jumped?.Invoke();
            }

            _isGrounded = isGrounded;
        }

        private void OnKeyboardUpdate()
        {
            // Move
            int move = (int)_keyboard.dKey.value - (int)_keyboard.aKey.value;
            float velocityX = move * m_moveSpeed;

            // Jump
            float velocityY = m_rigidbody2D.linearVelocityY;
            if (_isGrounded && _keyboard.spaceKey.wasPressedThisFrame) velocityY = m_jumpSpeed;

            m_rigidbody2D.linearVelocity = new(velocityX, velocityY);
        }

        private ContactFilter2D GroundFilter() 
        {
            return new() { layerMask = m_groundLayer };
        }
    }
}
