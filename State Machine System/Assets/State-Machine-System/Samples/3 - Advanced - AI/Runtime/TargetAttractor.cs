using UnityEngine;
using UnityEngine.InputSystem;

namespace M320.StateMachineSystem.Samples.AI
{
    public sealed class TargetAttractor : MonoBehaviour
    {
        [SerializeField, Range(0.001f, 50f)] private float m_forceConstant = 15f;
        [SerializeField, Range(0.001f, 1f)] private float m_smoothness = 0.1f;

        private GameObject[] _targets;
        private Camera _mainCamera;

        private void Awake()
        {
            _targets = GameObject.FindGameObjectsWithTag("Target");
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse == null) return;

            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(mouse.position.value);

            if (mouse.leftButton.isPressed) AttractTargets(mousePosition);
        }

        private void AttractTargets(Vector2 point) 
        {
            foreach (var target in _targets) 
            {
                if (!target.activeInHierarchy) continue;

                if (!target.TryGetComponent(out Rigidbody2D targetRigidbody2D)) continue;

                Vector2 delta = point - (Vector2)target.transform.position;
                float sqrDistance = delta.sqrMagnitude;

                Vector2 force = m_forceConstant / (sqrDistance * Mathf.Sqrt(sqrDistance) + m_smoothness) * delta;
                targetRigidbody2D.AddForce(force);
            }
        }
    }
}
