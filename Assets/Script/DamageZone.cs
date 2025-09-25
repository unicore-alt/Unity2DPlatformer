using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 10;
    [SerializeField] private float _damageCooldown = 1.5f;

    private readonly Dictionary<Collider2D, float> _damageTimer = new Dictionary<Collider2D, float>();

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            _damageTimer.TryAdd(other, -Mathf.Infinity);
            if (Time.time >= _damageTimer[other] + _damageCooldown)
            {
                if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
                {
                    playerHealth.TakeDamage(_damageAmount);
                    _damageTimer[other] = Time.time;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) => _damageTimer.Remove(other);
}