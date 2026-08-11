using UnityEngine;
using FirstGame.Interfaces;
using FirstGame.Combat;

namespace FirstGame.Players
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _moveSpeed = 5f;

        private IMovable _movable;
        private IRotatable _rotatable;
        private TargetDetector _targetDetector;
        private Health _health;
        private AnimationController _animationController;
        private DamageDealer _damageDealer;

        private float _damage = 15f;

        public Health GetHealthComponent() => _health;
        public bool IsAlive => _health.CheckValidHealth() > 0;

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _movable = GetComponent<IMovable>();
            _rotatable = GetComponent<IRotatable>();
            _targetDetector = GetComponent<TargetDetector>();
            _animationController = GetComponentInChildren<AnimationController>();
            _damageDealer = GetComponent<DamageDealer>();

            _health.Initialize(100f);

            _targetDetector.TargetDetected += Attack;
        }

        public void Move(Vector2 input)
        {
            Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
            Vector3 finalVelocity = Vector3.ClampMagnitude(moveDirection, 1f) * _moveSpeed;

            Rotate(moveDirection);
            _movable.Move(finalVelocity);

            bool isMoving = input.sqrMagnitude > 0.01f;

            _animationController.SetIsRunning(isMoving);
        }

        public void TakeDamage(float damage)
        {
            if (damage > 0)
            {
                _health.TakeDamage(damage);
                Debug.Log("МЕНЯ УДАРИЛИ");

                if (_health.CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }

        private void Rotate(Vector3 direction)
        {
            _rotatable.Rotate(direction);
        }

        private void Attack(IDamageable target)
        {
            if (_damageDealer.Attack(target, _damage))
            {
                _animationController.Attack();
                Debug.Log("Враг получил урон!");
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _targetDetector.TargetDetected -= Attack;
        }
    }
}
