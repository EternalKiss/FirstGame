using FirstGame.Interfaces;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class AreaChopAbility : AbilityBase
    {
        [SerializeField] private LayerMask _resourceLayer;
        [SerializeField] private float _radius = 2.5f;
        [SerializeField] private float _damage = 25f;

        private readonly Collider[] _hitColliders = new Collider[32];

        protected override void Activate()
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                transform.position,
                _radius,
                _hitColliders,
                _resourceLayer);

            for (int index = 0; index < hitCount; index++)
            {
                Collider hitCollider = _hitColliders[index];

                if (hitCollider == null)
                {
                    continue;
                }

                if (hitCollider.TryGetComponent(out IDamageable damageable))
                {
                    if (damageable.IsAlive)
                    {
                        damageable.TakeDamage(_damage);
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
