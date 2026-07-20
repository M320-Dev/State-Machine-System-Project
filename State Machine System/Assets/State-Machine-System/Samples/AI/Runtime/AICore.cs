using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class AICore : StateMachineCore<AICore>
    {
        public enum StateEnum
        {
            Wonder,
            Chase
        }

        #region Inspector Fields

        [Header("Components")]
        [SerializeField] private BoxCollider2D m_collider;
        [SerializeField] private Rigidbody2D m_rigidbody2D;

        [Header("Settings - Move")]
        [SerializeField, Range(0.1f, 20f)] private float m__moveDirectionSpeed = 5f;

        [Header("Settings - Jump")]
        [SerializeField, Range(0.1f, 20f)] private float m_jumpSpeed = 5f;

        [Header("Settings - Ground Check")]
        [SerializeField] private LayerMask m_groundLayer = 1;
        [SerializeField, Range(0.02f, 0.05f)] private float m_groundCheckDistance = 0.03f;
        [SerializeField, Range(0f, 0.1f)] private float m_groundCheckTolerance = 0.05f;

        [Header("Settings - Detection Distance")]
        [SerializeField, Range(2f, 20f)] private float m_detectionDistance = 5f;

        #endregion

        #region Cache Fields & Properties

        new public BoxCollider2D collider => m_collider;
        new public Rigidbody2D rigidbody2D => m_rigidbody2D;

        public bool isGrounded { get; private set; }
        public bool isWalledLeft { get; private set; }
        public bool isWalledRight { get; private set; }


        private readonly RaycastHit2D[] _oneHitBuffer = new RaycastHit2D[1];

        private int _moveDirection;

        #endregion

        protected override AICore Context => this;

        private GameObject[] _targets;
        private GameObject _closestVisibleTarget;

        protected override void Awake()
        {
            base.Awake();

            _targets = GameObject.FindGameObjectsWithTag("Target");
        }

        protected override IReadOnlyList<State> ConstructStates()
        {
            return new State[]
            {
                new WonderState(StateEnum.Wonder),
                new ChaseState(StateEnum.Chase)
            };
        }

        protected override void OnUpdate(float deltaTime)
        {
            CheckIfGrounded();
            CheckIfWalledLeft();
            CheckIfWalledEight();

            _closestVisibleTarget = GetClosestVisibleTarget();
            bool isOverClosestVisibleTarget = 
                _closestVisibleTarget != null && 
                m_collider.OverlapPoint(_closestVisibleTarget.transform.position);

            if (isOverClosestVisibleTarget) Destroy(_closestVisibleTarget);

            if (_closestVisibleTarget) ChangeState(StateEnum.Chase);
            else ChangeState(StateEnum.Wonder);
        }

        #region Checks

        private void CheckIfGrounded()
        {
            var filter = GroundFilter();
            int hitCount = m_collider.Cast(Vector2.down, filter, _oneHitBuffer, m_groundCheckDistance);
            isGrounded = hitCount > 0 && _oneHitBuffer[0].normal.y >= 1f - m_groundCheckTolerance;
        }
        private void CheckIfWalledLeft()
        {
            var filter = GroundFilter();
            int hitCount = m_collider.Cast(Vector2.left, filter, _oneHitBuffer, m_groundCheckDistance);
            isWalledLeft = hitCount > 0 && _oneHitBuffer[0].normal.x >= 1f - m_groundCheckTolerance;
        }
        private void CheckIfWalledEight()
        {
            var filter = GroundFilter();
            int hitCount = m_collider.Cast(Vector2.right, filter, _oneHitBuffer, m_groundCheckDistance);
            isWalledRight = hitCount > 0 && -_oneHitBuffer[0].normal.x >= 1f - m_groundCheckTolerance;
        }

        private ContactFilter2D GroundFilter()
        {
            return new() { layerMask = m_groundLayer };
        }

        #endregion

        public void MoveX()
        {
            m_rigidbody2D.linearVelocityX = _moveDirection * m__moveDirectionSpeed;
        }
        public void TryJump(bool jump)
        {
            if (isGrounded && jump)
            {
                m_rigidbody2D.linearVelocityY = m_jumpSpeed;
            }
        }

        public void RandomizeMoveDirection() 
        {
            _moveDirection = Random.value < 0.5f ? -1 : 1;
        }

        public void StopMovingX()
        {
            _moveDirection = 0;
            m_rigidbody2D.linearVelocityX = 0f;
        }

        public void TryFlipMoveDirection() 
        {
            if (_moveDirection > 0 && isWalledRight && !isWalledLeft ||
                _moveDirection < 0 && !isWalledRight && isWalledLeft) 
            {
                _moveDirection *= -1;
            }
        }

        public GameObject GetClosestVisibleTarget() 
        {
            if (_targets.Length <= 0) return null;

            GameObject closestVisibleTarget = null;
            float minDistance = float.PositiveInfinity;
            foreach (var target in _targets)
            {
                if (target == gameObject) continue;

                Vector2 delta = target.transform.position - transform.position;
                float distance = delta.magnitude;

                if (distance > m_detectionDistance) continue;

                var hit = Physics2D.Raycast(transform.position, delta, distance);
                if (hit && hit.transform.gameObject != target) continue;

                if (distance < minDistance) 
                {
                    closestVisibleTarget = target;
                    minDistance = distance;
                }
            }

            return closestVisibleTarget;
        }

        public void SetMoveDirectionTarget() 
        {
            float delta = _closestVisibleTarget.transform.position.x - transform.position.x;
            _moveDirection = System.Math.Sign(delta);
        }
    }
}
