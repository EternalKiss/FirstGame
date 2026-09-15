using FirstGame.Combat;
using FirstGame.Interfaces;
using FirstGame.Players.Abilities;
using FirstGame.Players.Weapon;
using UnityEngine;

namespace FirstGame.Players
{
    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _baseAttackInterval = 0.8f;
        [SerializeField] private float _damage = 70f;
        [SerializeField] private float _startHealth = 100f;
        [SerializeField] private float _attackSpeedMultiplier = 1f;

        private PlayerMovement _movement;
        private TargetDetector _targetDetector;
        private Health _health;
        private AnimationController _animationController;
        private DamageDealer _damageDealer;
        private PlayerCombatController _combatController;
        private PlayerDamageReceiver _damageReceiver;
        private AbilityController _abilityController;

        public Health GetHealthComponent() => _health;
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
            _abilityController = GetComponent<AbilityController>();

            if (_combatController != null)
            {
                _combatController.Initialize(_targetDetector, _animationController, _damageDealer, _baseAttackInterval, _damage);
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

        public void SetAttackSpeedMultiplier(float multiplier)
        {
            _attackSpeedMultiplier = multiplier;

            if (_combatController == null)
            {
                return;
            }

            float effectiveInterval = _baseAttackInterval / _attackSpeedMultiplier;
            _combatController.SetAttackInterval(effectiveInterval);
        }

        public void TakeDamage(float damage)
        {
            if (_damageReceiver != null)
            {
                _damageReceiver.ReceiveDamage(damage);
            }

            if (_abilityController != null)
            {
                _abilityController.TriggerAbilities(AbilityTrigger.OnDamageTaken);
            }
        }

        public void OnAttackHitEvent()
        {
            if (_combatController != null)
            {
                _combatController.OnAttackHitEvent();
            }
        }

        public void OnAbilityHitEvent()
        {
            if (_abilityController != null)
            {
                _abilityController.ApplyPendingDamage();
            }
        }

        public void SetIgnoreCombatAttackEvents(bool ignore)
        {
            if (_combatController != null)
            {
                _combatController.SetIgnoreAttackEvents(ignore);
            }
        }
    }
}
