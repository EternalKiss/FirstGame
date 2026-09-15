using FirstGame.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class ChainHit : AbilityBase
    {
        [SerializeField] private LayerMask _chainLayer;
        [SerializeField] private float _damage = 15f;
        [SerializeField] private float _chainRadius = 3f;
        [SerializeField] private int _maxChains = 3;

        private readonly Collider[] _hitColliders = new Collider[32];
        private readonly HashSet<Component> _alreadyHit = new HashSet<Component>();

        public override bool UsesMeleeAnimation => true;

        public override bool HasTargets()
        {
            return AbilityTargetFinder.FindClosestTarget(
                transform.position,
                _chainRadius,
                _chainLayer,
                _hitColliders,
                null) != null;
        }

        protected override void Activate() { }

        public override void OnApplyDamage()
        {
            _alreadyHit.Clear();

            IDamageable currentTarget = AbilityTargetFinder.FindClosestTarget(
                transform.position,
                _chainRadius,
                _chainLayer,
                _hitColliders,
                _alreadyHit);

            for (int chain = 0; chain < _maxChains; chain++)
            {
                if (currentTarget == null)
                {
                    break;
                }

                if (currentTarget is not Component currentComponent)
                {
                    break;
                }

                _alreadyHit.Add(currentComponent);
                currentTarget.TakeDamage(_damage);

                currentTarget = AbilityTargetFinder.FindClosestTarget(
                    currentComponent.transform.position,
                    _chainRadius,
                    _chainLayer,
                    _hitColliders,
                    _alreadyHit);
            }
        }
    }
}
