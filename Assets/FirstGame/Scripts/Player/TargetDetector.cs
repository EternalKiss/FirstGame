using FirstGame.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players
{
    public class TargetDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField] private float _detectionRadius = 5f;
        [SerializeField] private float _searchInterval = 0.1f;

        private readonly Collider[] _hitColliders = new Collider[20];
        private readonly List<IDamageable> _targetsInRange = new List<IDamageable>(20);
        private float _nextSearchTime;

        public IReadOnlyList<IDamageable> TargetsInRange
        {
            get
            {
                return _targetsInRange;
            }
        }

        private void Update()
        {
            if (Time.time >= _nextSearchTime)
            {
                _nextSearchTime = Time.time + _searchInterval;
                FindTargetsInRadius();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _detectionRadius);
        }

        private void FindTargetsInRadius()
        {
            _targetsInRange.Clear();
            Vector3 myPos = transform.position;

            int targetsCount = Physics.OverlapSphereNonAlloc(myPos, _detectionRadius, _hitColliders, _targetLayer);

            if (targetsCount == 0)
            {
                return;
            }

            for (int i = 0; i < targetsCount; i++)
            {
                Collider targetCollider = _hitColliders[i];

                if (targetCollider == null || targetCollider.gameObject.activeInHierarchy == false)
                {
                    continue;
                }

                if (targetCollider.TryGetComponent<IDamageable>(out var damageable))
                {
                    if (damageable.IsAlive)
                    {
                        _targetsInRange.Add(damageable);
                    }
                }
            }
        }
    }
}
