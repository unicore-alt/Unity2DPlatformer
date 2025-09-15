using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private float _moveInput;

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>().x;
    }

    private void Update()
    {
        transform.Translate(new Vector2(_moveInput, 0) * (_speed * Time.deltaTime));
    }
}

