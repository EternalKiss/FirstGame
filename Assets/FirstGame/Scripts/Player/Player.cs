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

        private float _damage = 15f;

        public Health GetHealthComponent() => _health;
        public bool IsAlive => _health.CheckValidHealth() > 0;

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _movable = GetComponent<IMovable>();
            _rotatable = GetComponent<IRotatable>();
            _targetDetector = GetComponent<TargetDetector>();

            _health.Initialize(100f);

            _targetDetector.TargetDetected += Attack;
        }

        public void Move(Vector2 input)
        {
            Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
            Vector3 finalVelocity = Vector3.ClampMagnitude(moveDirection, 1f) * _moveSpeed;

            Rotate(moveDirection);
            _movable.Move(finalVelocity);
        }

        public void TakeDamage(float damage)
        {
            if(damage > 0)
            {
                _health.TakeDamage(damage);
                Debug.Log("МЕНЯ УДАРИЛИ");
                
                if(_health.CurrentHealth <= 0)
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
            target.TakeDamage(_damage);
            Debug.Log("Враг получил урон!");
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
