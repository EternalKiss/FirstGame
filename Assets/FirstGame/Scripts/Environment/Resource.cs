using UnityEngine;
using FirstGame.Interfaces;
using FirstGame.Combat;
using System;

namespace FirstGame.Environment
{
    public class Resource : MonoBehaviour, IDamageable
    {
        private Health _health;
        private ResourceVisual _resourceVisual;

        public Health GetHealthComponent() => _health;
        public bool IsAlive => _health.CheckValidHealth() > 0;

        public event Action<Resource> OnDestroy;

        public void Initialize(float startHealth)
        {
            _health = GetComponent<Health>();
            _resourceVisual = GetComponent<ResourceVisual>();

            if (startHealth <= 0)
                Debug.Log("Health is less or equal 0!");

            _health.Initialize(startHealth);
        }

        public void TakeDamage(float damage)
        {
            _health.TakeDamage(damage);

            Debug.Log($"Получил урон. Мое здоровье {_health.CurrentHealth}");
        }
    }
}
