using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private IMovable _movable;
    private IRotatable _rotatable;

    private void Awake()
    {
        _movable = GetComponent<IMovable>();
        _rotatable = GetComponent<IRotatable>();
    }

    public void Move(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
        Vector3 finalVelocity = Vector3.ClampMagnitude(moveDirection, 1f) * _moveSpeed;

        _movable?.Move(finalVelocity);
    }
}
