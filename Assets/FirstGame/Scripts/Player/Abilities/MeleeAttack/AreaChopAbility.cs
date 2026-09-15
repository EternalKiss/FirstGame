using FirstGame.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class AreaChopAbility : AbilityBase
    {
        [SerializeField] private LayerMask _targetsLayer;
        [SerializeField] private float _radius = 2.5f;
        [SerializeField] private float _damage = 25f;

        private readonly Collider[] _hitColliders = new Collider[32];
        private readonly List<IDamageable> _foundTargets = new List<IDamageable>(32);

        public override bool UsesMeleeAnimation => true;

        public override bool HasTargets()
        {
            int count = AbilityTargetFinder.FindTargetsInRadius(
                transform.position,
                _radius,
                _targetsLayer,
                _hitColliders,
                _foundTargets);

            return count > 0;
        }

        protected override void Activate() { }

        public override void OnApplyDamage()
        {
            int count = AbilityTargetFinder.FindTargetsInRadius(
                transform.position,
                _radius,
                _targetsLayer,
                _hitColliders,
                _foundTargets);

            for (int index = 0; index < count; index++)
            {
                _foundTargets[index].TakeDamage(_damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}