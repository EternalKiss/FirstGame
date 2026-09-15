using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public enum AbilityTrigger
    {
        Interval = 0,
        OnDamageTaken = 1,
        OnNightStart = 2,
        OnLootPickup = 3
    }

    public abstract class AbilityBase : MonoBehaviour
    {
        [SerializeField] private AbilityTrigger _trigger = AbilityTrigger.Interval;
        [SerializeField] private float _cooldown = 5f;
        [SerializeField] private float _activeDuration = 0f;
        [SerializeField] private bool _isUnlocked = false;

        private float _cooldownRemaining;
        private float _activeTimeRemaining;
        private bool _isActive;

        public AbilityTrigger Trigger => _trigger;
        public float Cooldown => _cooldown;
        public float CooldownRemaining => _cooldownRemaining;
        public bool IsUnlocked => _isUnlocked;
        public bool IsActive => _isActive;
        public virtual bool UsesMeleeAnimation => false;

        public void Unlock() => _isUnlocked = true;

        public void SetCooldown(float newCooldown) => _cooldown = newCooldown;

        public void Tick(float deltaTime)
        {
            if (_cooldownRemaining > 0f)
            {
                _cooldownRemaining -= deltaTime;

                if (_cooldownRemaining < 0f)
                {
                    _cooldownRemaining = 0f;
                }
            }

            if (_isActive && _activeDuration > 0f)
            {
                _activeTimeRemaining -= deltaTime;

                if (_activeTimeRemaining <= 0f)
                {
                    _isActive = false;
                    Deactivate();
                }
            }
        }

        public bool TryActivate()
        {
            if (_isUnlocked == false) return false;
            if (_cooldownRemaining > 0f) return false;
            if (HasTargets() == false) return false;

            _cooldownRemaining = _cooldown;
            _isActive = true;
            _activeTimeRemaining = _activeDuration;

            Activate();
            return true;
        }

        public void ForceDeactivate()
        {
            if (_isActive == false) return;

            _isActive = false;
            _activeTimeRemaining = 0f;
            Deactivate();
        }

        public virtual bool HasTargets() => true;

        protected abstract void Activate();

        protected virtual void Deactivate() { }

        public virtual void OnApplyDamage() { }
    }
}
