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
        [SerializeField] private bool _isUnlocked = false;

        private float _cooldownRemaining;

        public AbilityTrigger Trigger => _trigger;
        public float Cooldown => _cooldown;
        public float CooldownRemaining => _cooldownRemaining;
        public bool IsUnlocked => _isUnlocked;

        public void Unlock()
        {
            _isUnlocked = true;
        }

        public void SetCooldown(float newCooldown)
        {
            _cooldown = newCooldown;
        }

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
        }

        public bool TryActivate()
        {
            if (_isUnlocked == false)
            {
                return false;
            }

            if (_cooldownRemaining > 0f)
            {
                return false;
            }

            _cooldownRemaining = _cooldown;
            Activate();
            return true;
        }

        protected abstract void Activate();
    }
}
