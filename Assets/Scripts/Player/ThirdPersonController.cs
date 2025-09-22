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
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        // Gravity/Jump disabled as per original
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
        [Tooltip("Maximum yaw rotation speed in degrees/second at full stick tilt.")]
        public float MaxTurnSpeed = 300f;
        [Tooltip("Ignore tiny stick twitches.")]
        public float RotationDeadzone = 0.2f;
        [Tooltip("Angular acceleration (deg/sec^2).")]
        public float TurnAccel = 720f;
        [Tooltip("Angular deceleration (deg/sec^2).")]
        public float TurnDecel = 1080f;
        [Tooltip("Stick angle (deg) from forward before rotation starts (angle deadzone).")]
        public float DeadzoneAngleDegrees = 8f;

        [Tooltip("Non-linear response for turn speed vs stick angle. Higher = slower when mostly forward.")]
        public float RotationCurveExponent = 1.6f;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
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

        // rotation smoothing
        private float _currentTurnSpeed; // current angular speed (deg/sec)

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        private void Awake()
        {
            if (Camera.main != null)
                _mainCamera = Camera.main.transform;
            else
                Debug.LogWarning("ThirdPersonController: No Camera.main found. Movement will not be camera-relative.");
        }

        private void Start()
        {
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets missing dependencies. Use Tools/Starter Assets/Reinstall Dependencies to fix.");
#endif

            AssignAnimationIDs();

            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            _input.analogMovement = true;
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
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
            // --- 1) Clamp input (no backward) and magnitude ---
            float forwardInput = Mathf.Max(0f, _input.move.y);
            Vector2 clampedInput = new Vector2(_input.move.x, forwardInput);
            float inputMagnitude = _input.analogMovement ? Mathf.Clamp01(clampedInput.magnitude) : 1f;

            // --- 2) Target speed ---
            float targetSpeed = (_input.sprint ? SprintSpeed : MoveSpeed) * inputMagnitude;
            if (inputMagnitude < 0.001f) targetSpeed = 0f;

            // Current horizontal velocity magnitude
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0f, _controller.velocity.z).magnitude;

            // Smooth to target speed
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

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // --- 3) Rotation with deadzone + accel/decel ---
            // --- Rotation with angle-based scaling + accel/decel + hard cap ---
            Vector2 rotInput = clampedInput;          // X = sideways, Y = forward (>= 0 due to clamp)
            float stickMag = Mathf.Clamp01(rotInput.magnitude);

            // A small magnitude deadzone
            if (stickMag < RotationDeadzone) rotInput = Vector2.zero;

            if (rotInput != Vector2.zero)
            {
                // Desired facing from input (camera-relative)
                Vector3 rotDir = new Vector3(rotInput.x, 0f, rotInput.y).normalized;
                _targetRotation = Mathf.Atan2(rotDir.x, rotDir.z) * Mathf.Rad2Deg;
                if (_mainCamera != null) _targetRotation += _mainCamera.eulerAngles.y;

                // *** Angle-based scaling ***
                // Angle of the stick away from pure forward (0°) toward pure sideways (90°)
                // Using the input space (X=side, Y=forward) so tiny sideways while mostly forward = small angle.
                float angleFromForwardDeg = Mathf.Abs(Mathf.Atan2(rotInput.x, Mathf.Max(0.0001f, rotInput.y))) * Mathf.Rad2Deg;

                // Map angle to [0..1] after a small angular deadzone
                float angleT = Mathf.InverseLerp(DeadzoneAngleDegrees, 90f, angleFromForwardDeg);
                angleT = Mathf.Clamp01(angleT);

                // Apply a response curve so near-forward turns are slower
                float angleScale = Mathf.Pow(angleT, RotationCurveExponent);

                // Combine with stick magnitude so light tilts also reduce speed
                float rotScale = angleScale * stickMag;

                // Target angular speed with a hard cap
                float targetTurnSpeed = MaxTurnSpeed * rotScale;

                // Turn accel/decel toward that target angular speed
                float desiredDelta = Mathf.DeltaAngle(transform.eulerAngles.y, _targetRotation);
                bool needsTurn = !Mathf.Approximately(desiredDelta, 0f);
                float accel = needsTurn ? TurnAccel : TurnDecel;
                _currentTurnSpeed = Mathf.MoveTowards(_currentTurnSpeed, targetTurnSpeed, accel * Time.deltaTime);

                // Step rotation, respecting current capped angular speed
                float currentYaw = transform.eulerAngles.y;
                float maxStep = _currentTurnSpeed * Time.deltaTime;
                float newYaw = Mathf.MoveTowardsAngle(currentYaw, _targetRotation, maxStep);
                transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
            }


            // --- 4) Move forward (camera-relative) ---
            Vector3 targetDirection = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;

            _controller.Move(
                targetDirection.normalized * (_speed * Time.deltaTime) +
                new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime
            );

            // --- 5) Animator updates ---
            if (_hasAnimator)
            {

                // Tell animator the player is moving at same normalized speed
                // Use run threshold so tempo is the run cycle, regardless of actual movement speed
                const float RUN_THRESHOLD = 1.9f;
                float animValue = (_input.move != Vector2.zero) ? RUN_THRESHOLD : 0f;

                _animator.SetFloat(_animIDSpeed, animValue, 0.1f, Time.deltaTime);
                _animator.SetFloat(_animIDMotionSpeed, 1f); // keep playback constant


                //_animator.SetFloat(_animIDSpeed, 1f); //_animationBlend);
                //_animator.SetFloat(_animIDMotionSpeed, 1f); //inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                if (_verticalVelocity < 0.0f)
                    _verticalVelocity = -2f;

                if (_jumpTimeoutDelta >= 0.0f)
                    _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                    _fallTimeoutDelta -= Time.deltaTime;
                else if (_hasAnimator)
                    _animator.SetBool(_animIDFreeFall, true);

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
                _verticalVelocity += Gravity * Time.deltaTime;
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0f, 1f, 0f, 0.35f);
            Color transparentRed = new Color(1f, 0f, 0f, 0.35f);

            Gizmos.color = Grounded ? transparentGreen : transparentRed;
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius
            );
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f && FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}
