using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Windows;

public class SimpleAIThirdPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed = 2f;
    public float SprintSpeed = 5f;
    public float RotationSmoothTime = 0.12f;
    public float SpeedChangeRate = 10f;
    [Range(0.1f,3f)]
    public float speedAnimate;

    [Header("Jumping")]
    public float JumpHeight = 1.2f;
    public float Gravity = -15f;
    public float JumpCooldown = 0.4f;
    public float JumpTimeout = 0.50f;

    [Header("Grounded")]
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;
    public float FallTimeout = 0.15f;
    private NavMeshAgent _controller;
    private Animator _animator;

    public float _speed;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53f;
    private bool _hasAnimator;

    private float _jumpCooldownDelta;

    // командные переменные от ИИ:
    private Vector2 _moveInput;
    private bool _run;

    public bool _jumpRequest;
    

    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private bool _isMoving;
    float _jumpTimeoutDelta;
    float _fallTimeoutDelta;
    private float _animationBlend;

    NavMeshAgent agent;
    private void Awake()
    {   
        agent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<NavMeshAgent>();
        TryGetComponent(out _animator);
        AssignAnimationIDs();
    }

    private void Start()
    {
        _fallTimeoutDelta = FallTimeout;
        _jumpTimeoutDelta = JumpTimeout;

    }

    private void Update()
    {
     
        float speed = _controller.velocity.magnitude / speedAnimate; 
        if (_animator) 
        {   _animator.SetFloat(_animIDSpeed, speed); 
            _animator.SetFloat(_animIDMotionSpeed, speed);
            _animator.SetBool(_animIDGrounded, Grounded); 
        }
        GroundedCheck();
        JumpAndGravity();
       
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
        Vector3 spherePos = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePos, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (_animator)
            _animator.SetBool(_animIDGrounded, Grounded);
    }

    private void Move()
    {
        // определяем целевую скорость
        float targetSpeed = _run ? SprintSpeed : MoveSpeed;
        if (_moveInput == Vector2.zero) targetSpeed = 0f;

        // текущая горизонтальная скорость
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        float inputMagnitude = _moveInput != Vector2.zero ? _moveInput.magnitude : 0f;

        // плавное ускорение/замедление
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        // animationBlend = правильное значение для Speed (именно его хочет аниматор)
        float animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (animationBlend < 0.01f) animationBlend = 0f;
        _animationBlend = animationBlend;

        // направление входа AI
        Vector3 inputDir = new Vector3(_moveInput.x, 0f, _moveInput.y).normalized;

        // поворот
        if (_moveInput != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation,
                ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, rotation, 0f);
        }

        // движение
        Vector3 targetDir = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
        _controller.Move(targetDir.normalized * (_speed * Time.deltaTime) +
                         new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);

        // обновляем Animator так же как StarterAssets
        if (_animator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
            }

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (_jumpRequest && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                agent.enabled = false;
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                    _jumpRequest = false;
                }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }

            // if we are not grounded, do not jump
            _jumpRequest = false;
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    // ------------ PUBLIC BOT COMMANDS -------------

    public void SetMove(Vector2 move) => _moveInput = move;
    public void SetRun(bool run) => _run = run;
    public void SetJump() => _jumpRequest = true;
}
