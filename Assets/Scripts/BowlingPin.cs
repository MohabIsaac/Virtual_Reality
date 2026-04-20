using UnityEngine;
using UnityEngine.Events;

namespace Bowling
{
    /// <summary>
    /// Detects when a bowling pin has fallen over based on its rotation.
    /// Manages its own registration and physical reset.
    /// </summary>
    public class BowlingPin : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField, Tooltip("Angle threshold from upright position to consider as fallen.")]
        private float angleThreshold = 45f;

        [Header("Debug Information")]
        [SerializeField, Tooltip("The current angle of the pin from its upright position (read-only).")]
        private float currentAngle;

        [Header("Events")]
        public UnityEvent OnPinFallen;

        private bool _isFallen;
        private Vector3 _initialUp;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;
        private Rigidbody _rb;

        private void Awake()
        {
            _initialUp = transform.up;
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            _rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            // Register self with the manager
            if (BowlingScoreManager.Instance != null)
            {
                BowlingScoreManager.Instance.RegisterPin(this);
            }
        }

        private void Update()
        {
            UpdateDebugInfo();

            if (!_isFallen && CheckIfFallen())
            {
                MarkAsFallen();
            }
        }

        private void UpdateDebugInfo()
        {
            currentAngle = Vector3.Angle(transform.up, _initialUp);
        }

        private bool CheckIfFallen()
        {
            return currentAngle > angleThreshold;
        }

        private void MarkAsFallen()
        {
            _isFallen = true;
            OnPinFallen?.Invoke();

            if (BowlingScoreManager.Instance != null)
            {
                BowlingScoreManager.Instance.IncrementScore();
            }
        }

        /// <summary>
        /// Physically resets the pin to its starting position and rotation.
        /// </summary>
        public void ResetToInitialPose()
        {
            _isFallen = false;

            // Stop any physics movement before resetting
            if (_rb != null)
            {
                _rb.linearVelocity = Vector3.zero;
                _rb.angularVelocity = Vector3.zero;
            }

            transform.position = _initialPosition;
            transform.rotation = _initialRotation;

            // Recalculate 'up' in case transform changed
            _initialUp = transform.up;
        }
    }
}
