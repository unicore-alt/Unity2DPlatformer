using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _moveDistance = 5f;
    private Vector2 _startPos;
    private bool _movingRight = true;
    private Rigidbody2D _rb;
    private void Start()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        Vector2 direction = _movingRight ? Vector2.right : Vector2.left;
        _rb.MovePosition(_rb.position + direction * (_speed * Time.deltaTime));
        _movingRight = _movingRight switch
        {
            true when _rb.position.x >= _startPos.x + _moveDistance => false,
            false when _rb.position.x <= _startPos.x - _moveDistance => true,
            _ => _movingRight
        };
    }
}
