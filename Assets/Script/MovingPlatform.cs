using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _moveDistance = 3f;
    [SerializeField] private float _moveSpeed = 2f;

    private Vector3 _startPos;
    private bool _movingRight = true;

    private void Start()
    {
        _startPos = transform.position;
    }

    private void Update()
    {
        float offset = _moveSpeed * Time.deltaTime;
        if (_movingRight)
        {
            transform.Translate(Vector2.right * offset);
            if (transform.position.x >= _startPos.x + _moveDistance)
                _movingRight = false;
        }
        else
        {
            transform.Translate(Vector2.left * offset);
            if (transform.position.x <= _startPos.x - _moveDistance)
                _movingRight = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Player))
            collision.transform.SetParent(transform);
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.Player))
            collision.transform.SetParent(null);
    }
}