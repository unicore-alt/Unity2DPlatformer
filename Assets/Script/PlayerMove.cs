using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 7f;
    [SerializeField] private float _moveThreshold = 0.01f;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundRadius = 0.2f;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Animation")]
    [SerializeField] private Animator _animator;
    private static readonly int _runAnim = Animator.StringToHash("Run");
    private static readonly int _jumpAnim = Animator.StringToHash("Jump");
    private static readonly int _verticalVelocity = Animator.StringToHash("VerticalVelocity");
    
    private Rigidbody2D _rb;
    private float _moveInput;
    private float _originalScaleX;
    private bool _isGrounded;
    private bool _jumpPressed;

    private PlayerInputActions _inputActions;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _originalScaleX = Mathf.Abs(transform.localScale.x);
        _inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Move.performed += OnMove;
        _inputActions.Player.Move.canceled += OnMoveCanceled;
        _inputActions.Player.Jump.started += OnJump;
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.performed -= OnMove;
        _inputActions.Player.Move.canceled -= OnMoveCanceled;
        _inputActions.Player.Jump.started -= OnJump;
        _inputActions.Disable();
    }
    
    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundRadius, _groundLayer);
        if (Mathf.Abs(_moveInput) > _moveThreshold)
        {
            float scaleX = _moveInput > 0 ? _originalScaleX : -_originalScaleX;
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        _animator.SetBool(_runAnim, Mathf.Abs(_moveInput) > _moveThreshold);
        _animator.SetBool(_jumpAnim, !_isGrounded);
        _animator.SetFloat(_verticalVelocity, _rb.linearVelocity.y);
        _rb.linearVelocity = new Vector2(_moveInput * _speed, _rb.linearVelocity.y);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _moveInput = input.x;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context) => _moveInput = 0f;

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && _isGrounded) _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _jumpForce);
    }
}