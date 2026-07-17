using UnityEngine;
using FirstGame.Interfaces;

namespace FirstGame.Enemy
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public void TakeDamage(float damage)
        {
            throw new System.NotImplementedException();
        }
    }
}
    