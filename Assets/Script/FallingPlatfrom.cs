using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _fallDelay = 0.7f;
    [SerializeField] private float _respawnDelay = 3f;
    [SerializeField] private float _shakeMagnitude = 0.05f;

    private Vector3 _originalPosition;
    private Rigidbody2D _rb;
    private Collider2D _col;
    private bool _isTriggered = false;

    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        _originalPosition = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _col.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (_isTriggered)
        {
            case false when collision.gameObject.CompareTag(Tags.Player):
                _isTriggered = true;
                StartCoroutine(ShakeAndFall());
                break;
        }
    }

    private IEnumerator ShakeAndFall()
    {
        float elapsed = 0f;
        while (elapsed < _shakeDuration)
        {
            transform.position = _originalPosition + (Vector3)Random.insideUnitCircle * _shakeMagnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = _originalPosition;
        yield return new WaitForSeconds(_fallDelay);
        _rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(_respawnDelay);
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        transform.position = _originalPosition;
        _isTriggered = false;
    }
}