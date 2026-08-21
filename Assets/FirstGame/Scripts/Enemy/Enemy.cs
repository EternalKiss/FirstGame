using FirstGame.Combat;
using FirstGame.Interfaces;
using FirstGame.Players;
using System;
using UnityEngine;

namespace FirstGame.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable, IDestructible
    {
        [SerializeField] private float _attackInterval = 0.8f;

        private float _attackRange = 4f;
        private float _damage = 15f;

        private Health _health;
        private Mover _mover;
        private DamageDealer _damageDealer;
        private PlayerDetector _playerDetector;
        private Rotator _rotator;
        private AnimationController _animationController;
        private EnemyVisual _enemyVisual;

        public Health GetHealthComponent() => _health;
        public bool IsAlive => _health.CheckValidHealth() > 0;

        public event Action<IDestructible> OnReadyToRelease;

        private void Awake()
        {
            _damageDealer = GetComponent<DamageDealer>();
            _playerDetector = GetComponent<PlayerDetector>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _rotator = GetComponent<Rotator>();
            _animationController = GetComponent<AnimationController>();
            _enemyVisual = GetComponent<EnemyVisual>();

            _animationController.AddAttackEventViaCode("Attack", "OnAttackHitEvent", 0.55f);
            _animationController.SynchronizeAnimationSpeed(_attackInterval, "Attack");
        }

        private void Update()
        {
            if (_health == null || _playerDetector == null || _mover == null) return;

            if (!IsAlive || !_playerDetector.HasTarget) return;

            Move(_playerDetector.GetPlayerPosition());
            Rotate(_playerDetector.GetPlayerPosition());

            if (_mover.TargetReached)
            {
                TryAttack();
            }
        }

        public void Initialize(float startHealth)
        {
            if (startHealth <= 0)
                Debug.Log("Health is less or equal 0!");

            _health.Initialize(startHealth);
        }

        public void TakeDamage(float damage)
        {
            if (!IsAlive) return;

            _health.TakeDamage(damage);

            if (_health.CurrentHealth > 0)
            {
                _enemyVisual?.PlayHitVisual();
            }
            else
            {
                _enemyVisual?.PlayDeathVisual();
                Die();
            }
        }

        public void Move(Vector3 target)
        {
            _mover.Move(target, _attackRange);
        }

        public void Rotate(Vector3 target)
        {
            _rotator.Rotate(target);
        }

        public void OnAttackHitEvent()
        {
            if (_playerDetector != null && _playerDetector.PlayerDamageable != null)
            {
                _playerDetector.PlayerDamageable.TakeDamage(_damage);
                Debug.Log("[Enemy] БУМ! Анимация врага завершила замах, игрок получил урон.");
            }
        }

        private void TryAttack()
        {
            if (_damageDealer.Attack(null, 0f))
            {
                _animationController.Attack();
            }
        }    

        private void Die()
        {
            OnReadyToRelease?.Invoke(this);
        }
    }
}
    