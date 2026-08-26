using UnityEngine;
using FirstGame.Interfaces;
using FirstGame.Combat;
using System;

namespace FirstGame.Environment
{
    public class Resource : MonoBehaviour, IDamageable, IDestructible
    {
        private Health _health;
        private ResourceVisual _resourceVisual;
        private Collider _collider;

        public Health GetHealthComponent() => _health;
        public bool IsAlive => _health.CheckValidHealth() > 0;

        public event Action<IDestructible> OnReadyToRelease;

        public void Initialize(float startHealth)
        {
            _health = GetComponent<Health>();
            _resourceVisual = GetComponentInChildren<ResourceVisual>();
            _collider = GetComponent<Collider>();

            if (startHealth <= 0)
                Debug.Log("Health is less or equal 0!");

            _health.Initialize(startHealth);
        }

        public void TakeDamage(float damage)
        {
            _health.TakeDamage(damage);

            if(_health.CurrentHealth > 0)
            {
                _resourceVisual.PlayHitVisual();
            }
            else
            {
                Die();
            }
        }

        public void CompleteDestruction()
        {
            OnReadyToRelease?.Invoke(this);
        }

        private void Die()
        {
            if (_collider != null)
            {
                _collider.enabled = false;
            }

            if (_resourceVisual != null)
            {
                _resourceVisual.PlayDeathVisual();
            }
            else
            {
                CompleteDestruction();
            }
        }
    }
}
