using FirstGame.Combat;
using UnityEngine;

namespace FirstGame.Players
{
    public class PlayerDamageReceiver : MonoBehaviour
    {
        private Health _health;
        private PlayerCombatController _combatController;

        public void Initialize(Health health, PlayerCombatController combatController, float startHealth)
        {
            _health = health;
            _combatController = combatController;

            _health.Initialize(startHealth);
        }

        public void ReceiveDamage(float damage)
        {
            if (damage <= 0)
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