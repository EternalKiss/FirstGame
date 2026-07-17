using FirstGame.Interfaces;
using UnityEngine;

namespace FirstGame.Combat
{
    public class Health : MonoBehaviour
    {
        private float _maxHealth;
        private float _currentHealth;

        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        public void Initialize(float maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;

            _currentHealth = CheckValidHealth();
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
        }
    }
}