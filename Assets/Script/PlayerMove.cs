using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _jumpForce = 8f;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveThreshold = 0.01f;

    private static readonly int RunAnim = Animator.StringToHash("Run");

    private Rigidbody2D _rb;
    private float _moveInput;
    private float _originalScaleX;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.freezeRotation = true;
        _originalScaleX = Mathf.Abs(transform.localScale.x);
    }

    private void Update()
    {
        _moveInput = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) _moveInput = -1f;
            else if (Keyboard.current.dKey.isPressed) _moveInput = 1f;
        }
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(_moveInput) > _moveThreshold)
        {
            float scaleX = _moveInput > 0 ? _originalScaleX : -_originalScaleX;
            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        _animator.SetBool(RunAnim, Mathf.Abs(_moveInput) > _moveThreshold);
        _rb.linearVelocity = new Vector2(_moveInput * _speed, _rb.linearVelocity.y);
        
    }

}
