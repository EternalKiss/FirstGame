using FirstGame.Combat;
using UnityEngine;

namespace FirstGame.Players
{
    public class PlayerDamageReceiver : MonoBehaviour
    {
        private Health _health;
        private PlayerCombatController _combatController;

        private bool _isShielded;

        public void Initialize(Health health, PlayerCombatController combatController, float startHealth)
        {
            _health = health;
            _combatController = combatController;

            _health.Initialize(startHealth);
        }

        public void SetShielded(bool isShielded)
        {
            _isShielded = isShielded;
        }

        public void ReceiveDamage(float damage)
        {
            if (damage <= 0)
            {
                return;
            }

            if (_isShielded)
            {
                return;
            }

            _health.TakeDamage(damage);

            if (_health.CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                if (_combatController != null)
                {
                    _combatController.InterruptAttack();
                }
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}