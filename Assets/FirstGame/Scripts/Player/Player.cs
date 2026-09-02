using FirstGame.Combat;
using FirstGame.Interfaces;
using FirstGame.Players.Weapon;
using System;
using UnityEngine;

namespace FirstGame.Players
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _attackInterval = 0.8f;
        [SerializeField] private float _damage = 70f;
        [SerializeField] private float _startHealth = 100f;

        private PlayerMovement _movement;
        private TargetDetector _targetDetector;
        private Health _health;
        private AnimationController _animationController;
        private DamageDealer _damageDealer;
        private PlayerCombatController _combatController;
        private PlayerDamageReceiver _damageReceiver;

        public Health GetHealthComponent()
        {
            return _health;
        }

        public bool IsAlive
        {
            get
            {
                return _health != null && _health.CheckValidHealth() > 0;
            }
        }

        public void Initialize()
        {
            _health = GetComponent<Health>();
            _targetDetector = GetComponent<TargetDetector>();
            _damageDealer = GetComponent<DamageDealer>();
            _movement = GetComponent<PlayerMovement>();
            _animationController = GetComponentInChildren<AnimationController>();

            _combatController = GetComponent<PlayerCombatController>();
            _damageReceiver = GetComponent<PlayerDamageReceiver>();

            if (_combatController != null)
            {
                _combatController.Initialize(_targetDetector, _animationController, _damageDealer, _attackInterval, _damage);
            }

            if (_damageReceiver != null)
            {
                _damageReceiver.Initialize(_health, _combatController, _startHealth);
            }

            if (_movement != null)
            {
                _movement.Initialize(_animationController);
            }

            var weaponVisual = GetComponentInChildren<WeaponVisual>();
            if (weaponVisual != null)
            {
                weaponVisual.BindToPlayer(this);
            }
        }

        public void Move(Vector2 input)
        {
            if (_movement != null)
            {
                _movement.Move(input);
            }
        }

        public void TakeDamage(float damage)
        {
            if (_damageReceiver != null)
            {
                _damageReceiver.ReceiveDamage(damage);
            }
        }
        public void OnAttackHitEvent()
        {
            if (_combatController != null)
            {
                _combatController.OnAttackHitEvent();
            }
        }
    }
}
