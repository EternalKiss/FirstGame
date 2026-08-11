using FirstGame.Combat;
using FirstGame.Interfaces;
using FirstGame.Players;
using System;
using UnityEngine;

namespace FirstGame.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable, IDestructible
    {
        private float _attackRange = 4f;
        private float _damage = 15f;

        private Health _health;
        private Mover _mover;
        private DamageDealer _damageDealer;
        private PlayerDetector _playerDetector;
        private Rotator _rotator;
        private AnimationController _animationController;

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
            _health.TakeDamage(damage);

            if (_health.CurrentHealth <= 0)
            {
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

        private void TryAttack()
        {
            _damageDealer.Attack(_playerDetector.PlayerDamageable, _damage);
            _animationController.Attack();
        }    

        private void Die()
        {
            OnReadyToRelease?.Invoke(this);
        }
    }
}
    