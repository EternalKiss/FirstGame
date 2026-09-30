using System;
using UnityEngine;

namespace FirstGame.Combat
{
    public class Health : MonoBehaviour
    {
        private float _maxHealth;
        private float _currentHealth;

        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        public event Action<float> HealthChanged;
        public event Action Died;

        public void Initialize(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;

            _currentHealth = CheckValidHealth();

            HealthChanged?.Invoke(_currentHealth);
        }

        public float CheckValidHealth()
        {
            if (_currentHealth > _maxHealth)
            {
                return _maxHealth;
            }

            if (_currentHealth < 0)
            {
                return 0;
            }

            return _currentHealth;
        }

        public void TakeDamage(float damage)
        {
            if (damage > 0)
            {
                _currentHealth -= damage;
                _currentHealth = CheckValidHealth();
            }

            HealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0f)
            {
                Died?.Invoke();
            }

            Debug.Log(_currentHealth);
        }

        public void Heal(float amount)
        {
            if (amount <= 0f)
            {
                return;
            }

            if (_currentHealth <= 0f)
            {
                return;
            }

            _currentHealth += amount;
            _currentHealth = CheckValidHealth();

            HealthChanged?.Invoke(_currentHealth);
        }

        public void Revive(float amount)
        {
            float healthRevive = _maxHealth / amount;

            _currentHealth = healthRevive;
            _currentHealth = CheckValidHealth();
            Debug.Log(_currentHealth);
            HealthChanged?.Invoke(_currentHealth);
        }
    }
}