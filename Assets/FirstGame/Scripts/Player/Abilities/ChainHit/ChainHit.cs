using FirstGame.Interfaces;
using FirstGame.Sound;
using System.Collections;
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
        private readonly List<Vector3> _chainPositions = new List<Vector3>(8);
        private readonly List<IDamageable> _chainTargets = new List<IDamageable>(8);

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

        protected override void Activate()
        {
        }

        public override void OnApplyDamage()
        {
            _alreadyHit.Clear();
            _chainPositions.Clear();
            _chainTargets.Clear();

            _chainPositions.Add(transform.position);

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
                _chainPositions.Add(currentComponent.transform.position);
                _chainTargets.Add(currentTarget);

                currentTarget = AbilityTargetFinder.FindClosestTarget(
                    currentComponent.transform.position,
                    _chainRadius,
                    _chainLayer,
                    _hitColliders,
                    _alreadyHit);
            }

            StartCoroutine(ApplyChainRoutine());
        }

        private IEnumerator ApplyChainRoutine()
        {
            if (_chainTargets.Count == 0)
            {
                yield break;
            }

            if (_chainPositions.Count > 1)
            {
                ChainLightningVfx.Play(_chainPositions);
            }

            for (int index = 0; index < _chainTargets.Count; index++)
            {
                if (AudioService.Instance != null)
                {
                    AudioService.Instance.PlayChainHit();
                }

                IDamageable target = _chainTargets[index];

                if (target != null && target.IsAlive)
                {
                    target.TakeDamage(_damage);
                }

                if (index < _chainTargets.Count - 1)
                {
                    yield return new WaitForSeconds(ChainLightningVfx.SegmentDelay);
                }
            }
        }

        public override void OnImpact(IDamageable target)
        {
        }
    }
}
