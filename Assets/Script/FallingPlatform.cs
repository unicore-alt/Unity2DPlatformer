using UnityEngine;
using System.Collections;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float _fallDelay = 1f;
    [SerializeField] private float _respawnDelay = 3f;
    [SerializeField] private float _shakeDuration = 0.5f;
    [SerializeField] private float _shakeMagnitude = 0.05f;
    [SerializeField] private LayerMask _playerLayer;

    private Rigidbody2D _rb;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool hasTriggeredFall = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        _rb.bodyType = RigidbodyType2D.Static;
        _rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        switch (hasTriggeredFall)
        {
            case false:
            {
                Collider2D hit = Physics2D.OverlapBox(transform.position, transform.localScale, 0f, _playerLayer);
                if (hit != null && hit.CompareTag("Player"))
                {
                    hasTriggeredFall = true;
                    StartCoroutine(FallSequence());
                }
                break;
            }
        }
    }

    IEnumerator FallSequence()
    {
        yield return new WaitForSeconds(_fallDelay);

        float elapsed = 0f;
        while (elapsed < _shakeDuration)
        {
            Vector3 offset = new Vector3(
                Random.Range(-1f, 1f) * _shakeMagnitude,
                Random.Range(-1f, 1f) * _shakeMagnitude,
                0f
            );
            transform.position = _startPosition + offset;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _startPosition;
        _rb.bodyType = RigidbodyType2D.Dynamic;

        yield return new WaitForSeconds(_respawnDelay);
        Respawn();
    }

    private void Respawn()
    {
        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Static;
        transform.position = _startPosition;
        transform.rotation = _startRotation;
        hasTriggeredFall = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}