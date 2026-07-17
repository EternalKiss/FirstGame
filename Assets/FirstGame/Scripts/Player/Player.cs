using UnityEngine;
using FirstGame.Interfaces;

namespace FirstGame.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;

        private IMovable _movable;
        private IRotatable _rotatable;
        private TargetDetector _targetDetector;

        private float _damage = 15f;

        private void Awake()
        {
            _movable = GetComponent<IMovable>();
            _rotatable = GetComponent<IRotatable>();
            _targetDetector = GetComponent<TargetDetector>();
        }

        private void OnEnable()
        {
            _targetDetector.TargetDetected += Attack;
        }

        private void OnDisable()
        {
            _targetDetector.TargetDetected -= Attack;
        }

        public void Move(Vector2 input)
        {
            Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
            Vector3 finalVelocity = Vector3.ClampMagnitude(moveDirection, 1f) * _moveSpeed;

            Rotate(moveDirection);
            _movable.Move(finalVelocity);
        }

        private void Rotate(Vector3 direction)
        {
            _rotatable.Rotate(direction);
        }

        private void Attack(IDamageable target)
        {
            target.TakeDamage(_damage);
            Debug.Log("Враг получил урон!");
        }
    }
}
