using UnityEngine;

public class DamageFlashTryGet : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.red;
    [SerializeField] private float _flashDuration = 0.2f;

    private Color _originalColor;
    private Renderer _rend;

    private void Awake()
    {
        if (TryGetComponent<Renderer>(out _rend))
            _originalColor = _rend.material.color;
    }

    public void Flash()
    {
        if (_rend != null)
            StartCoroutine(FlashRoutine());
    }
    
    private System.Collections.IEnumerator FlashRoutine()
    {
        _rend.material.color = _flashColor;
        yield return new WaitForSeconds(_flashDuration);
        _rend.material.color = _originalColor;
    }
}