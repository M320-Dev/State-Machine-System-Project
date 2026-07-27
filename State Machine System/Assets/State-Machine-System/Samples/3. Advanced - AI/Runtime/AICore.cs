using System.Collections.Generic;
using UnityEngine;

namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class AICore : StateMachineCore<AICore>
    {
        public enum StateEnum
        {
            Wander,
            Chase
        }

        #region Inspector Fields

        [Header("Components")]
        [SerializeField] private BoxCollider2D m_collider;
        [SerializeField] private Rigidbody2D m_rigidbody2D;

        [Header("Settings - Move")]
        [SerializeField, Range(0.1f, 20f)] private float m_wonderMoveSpeed = 3f;
        [SerializeField, Range(0.1f, 20f)] private float m_chaseMoveSpeed = 5f;

        [Header("Settings - Jump")]
        [SerializeField, Range(0.1f, 20f)] private float m_jumpSpeed = 5f;

        [Header("Settings - Ground Check")]
        [SerializeField] private LayerMask m_groundLayer = 1;
        [SerializeField, Range(0.02f, 0.05f)] private float m_groundCheckDistance = 0.03f;
        [SerializeField, Range(0f, 0.1f)] private float m_groundCheckTolerance = 0.05f;

        [Header("Settings - Detection Distance")]
        [SerializeField, Range(2f, 20f)] private float m_detectionRadius = 5f;

        [Header("Settings - Durations - Wander")]
        [SerializeField, Range(0.001f, 10f)] private float m_minIdle = 0.5f;
        [SerializeField, Range(0.001f, 10f)] private float m_maxIdle = 2f;
        [SerializeField, Range(0.001f, 10f)] private float m_minMove = 0.5f;
        [SerializeField, Range(0.001f, 10f)] private float m_maxMove = 2f;

        [Header("Settings - Durations - Chase")]
        [SerializeField, Range(0.001f, 10f)] private float m_alerted = 1f;
        [SerializeField, Range(0.001f, 10f)] private float m_failed = 1f;
        [SerializeField, Range(0.001f, 10f)] private float m_succeeded = 1f;

        #endregion

        #region Inspector Properties

        // Settings - Durations - Wander
        public float minIdle => m_minIdle;
        public float maxIdle => m_maxIdle;
        public float minMove => m_minMove;
        public float maxMove => m_maxMove;

        // Settings - Durations - Chase
        public float alerted => m_alerted;
        public float failed => m_failed;
        public float succeeded => m_succeeded;

        #endregion

        #region Cache Fields & Properties

        new public BoxCollider2D collider => m_collider;
        new public Rigidbody2D rigidbody2D => m_rigidbody2D;

        public bool isGrounded { get; private set; }
        public bool isWalledLeft { get; private set; }
        public bool isWalledRight { get; private set; }


        private readonly RaycastHit2D[] _oneHitBuffer = new RaycastHit2D[1];

        private int _moveDirection;

        private GameObject[] _targets;
        private GameObject _closestVisibleTarget;
        public bool seeingClosestVisibleTarget => _closestVisibleTarget;
        public bool isOverClosestVisibleTarget { get; private set; }

        #endregion

        #region States

        public WanderState Wander { get; private set; }
        public ChaseState Chase { get; private set; }

        #endregion

        protected override AICore Context => this;

        protected override void Awake()
        {
            base.Awake();

            _targets = GameObject.FindGameObjectsWithTag("Target");
        }

        protected override IEnumerable<StateDefinition> DefineStates()
        {
            return new StateDefinition[] 
            {
                StateDefinition.Create<State>(new(StateEnum.Wander)),
                StateDefinition.Create<State>(new(StateEnum.Chase))
            };
        }

        protected override void OnUpdate(float deltaTime)
        {
            CheckIfGrounded();
            CheckIfWalledLeft();
            CheckIfWalledEight();

            _closestVisibleTarget = GetClosestVisibleTarget();
            isOverClosestVisibleTarget = 
                _closestVisibleTarget != null && 
                m_collider.OverlapPoint(_closestVisibleTarget.transform.position);

            if (_closestVisibleTarget && CurrentState == Wander) ChangeState(Chase);
            else ChangeState(Wander);
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

        private float MoveSpeed()
        {
            if (CurrentState == Wander) return m_wonderMoveSpeed;
            else if (CurrentState == Chase) return m_chaseMoveSpeed;
            return 0f;
        }

        public void MoveTowardsTargetX()
        {
            MoveTowardsX(_closestVisibleTarget 
                ? _closestVisibleTarget.transform.position.x 
                : transform.position.x);
        }
        public void MoveTowardsX(float xTarget)
        {
            float delta = xTarget - transform.position.x;
            float sqrDelta = delta * delta;

            float desiredVelocity = System.MathF.Sign(delta) * MoveSpeed();

            float desiredDelta = desiredVelocity * Time.fixedDeltaTime;
            float sqrDesiredDelta = desiredDelta * desiredDelta;

            if (sqrDesiredDelta > sqrDelta) 
            {
                desiredVelocity *= Mathf.Sqrt(sqrDelta / sqrDesiredDelta);
            }

            m_rigidbody2D.linearVelocityX = desiredVelocity;
        }
        public void MoveX()
        {
            m_rigidbody2D.linearVelocityX = _moveDirection * MoveSpeed();
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
                if (!target.activeInHierarchy || target == gameObject) continue;

                Vector2 delta = target.transform.position - transform.position;
                float distance = delta.magnitude;

                if (distance > m_detectionRadius) continue;

                var hitCount = m_collider.Raycast(delta, _oneHitBuffer, distance);

                if (hitCount > 0 && _oneHitBuffer[0].transform.gameObject != target) continue;

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

        public void DisableClosestVisibleTarget() 
        {
            _closestVisibleTarget.SetActive(false);
        }

        public float RandomIdleDuration() 
        {
            return Random.Range(m_minIdle, m_maxIdle);
        }
        public float RandomMoveDuration()
        {
            return Random.Range(m_minMove, m_maxMove);
        }
    }
}
