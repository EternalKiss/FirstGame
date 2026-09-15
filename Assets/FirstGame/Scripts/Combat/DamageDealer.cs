using FirstGame.Interfaces;
using UnityEngine;

namespace FirstGame.Combat
{
    public class DamageDealer : MonoBehaviour
    {
        public void DealDamage(IDamageable target, float damage)
        {
            if (target == null)
            {
                return;
            }

            if (target.IsAlive == false)
            {
                return;
            }

            target.TakeDamage(damage);
        }
    }
}