using JetBrains.Annotations;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.Events;
#endif

[System.Serializable]
public class FloatEvent : UnityEvent<float> { }

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        private float Gravity = 0.0f;
        private float JumpTimeout = 0.0f;
        private float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        [Header("Measurement Events")]
        public FloatEvent OnWalkUpdate;

        [Header("Rotation Tuning")]
        public float MaxTurnSpeed = 300f;
        public float RotationDeadzone = 0.2f;
        public float TurnAccel = 720f;
        public float TurnDecel = 1080f;
        public float DeadzoneAngleDegrees = 8f;
        public float RotationCurveExponent = 1.6f;

        // 🔹 RUN TOGGLE
        [Header("Run Toggle")]
        public Key RunToggleKey = Key.LeftAlt; // ✅ CHANGED HERE
        private bool _runToggled;

        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;

        private bool _hasAnimator;
        private Transform _mainCamera;
        private float _currentTurnSpeed;

        private void Awake()
        {
            if (Camera.main != null)
                _mainCamera = Camera.main.transform;
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#endif
            AssignAnimationIDs();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            _input.analogMovement = true;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            HandleRunToggle();

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        private void HandleRunToggle()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null && Keyboard.current[RunToggleKey].wasPressedThisFrame)
            {
                _runToggled = !_runToggled;
            }
#endif
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            if (_hasAnimator)
                _animator.SetBool(_animIDGrounded, Grounded);
        }

        private void Move()
        {
            float forwardInput = Mathf.Max(0f, _input.move.y);
            Vector2 clampedInput = new Vector2(_input.move.x, forwardInput);
            float inputMagnitude = _input.analogMovement ? Mathf.Clamp01(clampedInput.magnitude) : 1f;

            bool isRunning = _runToggled || _input.sprint;
            float targetSpeed = (isRunning ? SprintSpeed : MoveSpeed) * inputMagnitude;
            if (inputMagnitude < 0.001f) targetSpeed = 0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;

            const float speedOffset = 0.1f;
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector2 rotInput = clampedInput;
            float stickMag = Mathf.Clamp01(rotInput.magnitude);
            if (stickMag < RotationDeadzone) rotInput = Vector2.zero;

            if (rotInput != Vector2.zero)
            {
                Vector3 rotDir = new Vector3(rotInput.x, 0f, rotInput.y).normalized;
                _targetRotation = Mathf.Atan2(rotDir.x, rotDir.z) * Mathf.Rad2Deg;
                if (_mainCamera != null) _targetRotation += _mainCamera.eulerAngles.y;

                float angleFromForwardDeg = Mathf.Abs(Mathf.Atan2(rotInput.x, Mathf.Max(0.0001f, rotInput.y))) * Mathf.Rad2Deg;
                float angleT = Mathf.InverseLerp(DeadzoneAngleDegrees, 90f, angleFromForwardDeg);
                angleT = Mathf.Clamp01(angleT);

                float rotScale = Mathf.Pow(angleT, RotationCurveExponent) * stickMag;
                float targetTurnSpeed = MaxTurnSpeed * rotScale;

                _currentTurnSpeed = Mathf.MoveTowards(_currentTurnSpeed, targetTurnSpeed, TurnAccel * Time.deltaTime);
                float newYaw = Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation, _currentTurnSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + Vector3.up * _verticalVelocity * Time.deltaTime);

            if (_hasAnimator)
            {
                const float RUN_THRESHOLD = 1.9f;
                float animValue = (_input.move != Vector2.zero) ? RUN_THRESHOLD : 0f;
                _animator.SetFloat(_animIDSpeed, animValue, 0.1f, Time.deltaTime);
                _animator.SetFloat(_animIDMotionSpeed, 1f);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                if (_verticalVelocity < 0.0f)
                    _verticalVelocity = -2f;
            }
        }
    }
}
