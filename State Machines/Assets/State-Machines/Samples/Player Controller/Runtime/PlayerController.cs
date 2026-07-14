using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace M320.StateMachines.Samples.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        #region Inspector Fields

        [Header("Components")]
        [SerializeField] private BoxCollider2D m_collider;

        [Header("Settings - Move")]
        [SerializeField, Range(0.1f, 20f)] private float m_moveSpeed = 5f;
        [SerializeField, Range(-1, 1)] private int m_forceMove = 0;

        [Header("Settings - Jump")]
        [SerializeField, Range(0.1f, 20f)] private float m_jumpSpeed = 5f;

        [Header("Settings - Gravity")]
        [SerializeField, Range(0.1f, 20f)] private float m_gravityMultiplier = 1f;

        [Header("Settings - Ground Check")]
        [SerializeField, Range(0.001f, 0.5f)] private float m_groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask m_groundLayer = 1;

        #endregion

        #region Cache Fields & Properties

        private Keyboard _keyboard;

        private Vector2 position 
        {
            get => transform.position;
            set => transform.position = (Vector3)value;
        }
        private Vector2 _velocity;

        private bool _isGrounded;
        private readonly RaycastHit2D[] _oneHitBuffer = new RaycastHit2D[1];
        private readonly List<RaycastHit2D> _allHitBuffer = new();
        private readonly List<Collider2D> _allOverlapBuffer = new();

        #endregion

        #region Events

        public event Action Jumped;
        public event Action Landed;

        #endregion

        private void Update()
        {
            CheckIfGrounded();

            _keyboard = Keyboard.current;

            if (_keyboard != null) OnKeyboardUpdate();
            else 
            {
                _velocity.x = 0f;
            }
        }

        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;

            if (!_isGrounded) _velocity.y += Physics2D.gravity.y * m_gravityMultiplier * dt;

            Vector2 displacement = _velocity * dt;
            displacement = HandleAnticipatedCollisions(displacement);

            Vector2 desiredPosition = position + displacement;
            desiredPosition = HandleOverlaps(desiredPosition);

            position = desiredPosition;
        }

        private void CheckIfGrounded()
        {
            var filter = GroundFilter();
            int hitCount = m_collider.Cast(Vector2.down, filter, _oneHitBuffer, m_groundCheckDistance);
            bool isGrounded = hitCount > 0 && _oneHitBuffer[0].normal.y == 1f;

            if (!_isGrounded && isGrounded) 
            {
                _velocity.y = 0f;

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
            int move;
            if (m_forceMove == 0) move = (int)_keyboard.dKey.value - (int)_keyboard.aKey.value;
            else move = m_forceMove;
            _velocity.x = move * m_moveSpeed;

            // Jump
            if (_isGrounded && _keyboard.spaceKey.wasPressedThisFrame) _velocity.y = m_jumpSpeed;
        }

        private Vector2 HandleAnticipatedCollisions(Vector2 displacement)
        {
            if (displacement == Vector2.zero) return displacement;

            var filter = GroundFilter();
            _allHitBuffer.Clear();
            m_collider.Cast(displacement, filter, _allHitBuffer);

            float minDistance = float.MaxValue;

            foreach (var hit in _allHitBuffer)
            {
                minDistance = Mathf.Min(minDistance, hit.distance);
            }

            if (minDistance == float.MaxValue) return displacement;

            return Vector2.ClampMagnitude(displacement, minDistance);
        }
        private Vector2 HandleOverlaps(Vector2 desiredPosition)
        {
            var filter = GroundFilter();
            _allOverlapBuffer.Clear();
            m_collider.Overlap(desiredPosition, 0f, filter, _allOverlapBuffer);

            foreach (var collider in _allOverlapBuffer)
            {
                var distance = m_collider.Distance(collider);
                desiredPosition += distance.distance * distance.normal;
            }

            return desiredPosition;
        }

        private ContactFilter2D GroundFilter() 
        {
            return new() { layerMask = m_groundLayer };
        }
    }
}
