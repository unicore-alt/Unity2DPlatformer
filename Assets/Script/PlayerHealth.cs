using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;
    [SerializeField] private HealthBar _healthBar;
    private void Start()
    {
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
    }

    private void Update()
    {
        if (_currentHealth <= 0)
            _currentHealth = _maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        if (TryGetComponent<DamageFlashTryGet>(out var flash))
            flash.Flash();
        _currentHealth -= damage;
        _healthBar.SetHealth(_currentHealth);
    }
}
