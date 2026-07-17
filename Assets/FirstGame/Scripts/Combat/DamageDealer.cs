using FirstGame.Interfaces;
using UnityEngine;

namespace FirstGame.Combat
{
    public class DamageDealer : MonoBehaviour
    {
        private float _attackDelay;
        private float _attackInterval = 1f;

       public void Attack(IDamageable target, float damage)
        {
            if (Time.time >= _attackDelay)
            {
                _attackDelay = Time.time + _attackInterval;
                target.TakeDamage(damage);
            }
        }
    }
}
