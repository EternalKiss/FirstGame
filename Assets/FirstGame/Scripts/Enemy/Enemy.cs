using FirstGame.Combat;
using FirstGame.Interfaces;
using FirstGame.Players;
using System;
using TMPro;
using UnityEngine;

namespace FirstGame.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable, IDestructible
    {
        [SerializeField] private float _attackInterval = 0.8f;
        [SerializeField] private float _attackRange = 4f;
        [SerializeField] private float _damage = 15f;

        private Health _health;
        private Mover _mover;
        private DamageDealer _damageDealer;
        private PlayerDetector _playerDetector;
        private Rotator _rotator;
        private AnimationController _animationController;
        private EnemyVisual _enemyVisual;

        private float _nextAttackTime;
        private bool _isAttacking;

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
            if (_health == null || _playerDetector == null || _mover == null)
            {
                return;
            }

            if (IsAlive == false || _playerDetector.HasTarget == false)
            {
                return;
            }

            Vector3 targetPosition = _playerDetector.GetPlayerPosition();

            if (_isAttacking && Time.time >= _nextAttackTime)
            {
                _isAttacking = false;
            }

            if (IsTargetClose(targetPosition))
            {
                Rotate(targetPosition);
                TryAttack();
            }
            else
            {
                Move(targetPosition);
                Rotate(targetPosition);
            }
        }

        public void Initialize(float startHealth)
        {
            if (startHealth <= 0f)
            {
                Debug.Log("Health is less or equal 0!");
            }

            _health.Initialize(startHealth);
        }

        public void TakeDamage(float damage)
        {
            if (IsAlive == false)
            {
                return;
            }

            _health.TakeDamage(damage);

            if (_health.CurrentHealth > 0f)
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
            if (_playerDetector == null)
            {
                return;
            }

            if (_playerDetector.PlayerDamageable == null)
            {
                return;
            }

            _damageDealer.DealDamage(_playerDetector.PlayerDamageable, _damage);
        }

        private bool IsTargetClose(Vector3 targetPosition)
        {
            Vector3 offset = targetPosition - transform.position;
            offset.y = 0f;

            float sqrDistance = offset.sqrMagnitude;

            return sqrDistance <= _attackRange * _attackRange;
        }

        private void TryAttack()
        {
            if (_isAttacking)
            {
                return;
            }

            if (Time.time < _nextAttackTime)
            {
                return;
            }

            _isAttacking = true;
            _nextAttackTime = Time.time + _attackInterval;

            _animationController.Attack();
        }

        private void Die()
        {
            OnReadyToRelease?.Invoke(this);
        }
    }
}
    