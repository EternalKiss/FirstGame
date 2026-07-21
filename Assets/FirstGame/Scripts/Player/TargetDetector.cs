using FirstGame.Interfaces;
using System;
using UnityEngine;

namespace FirstGame.Players
{
    public class TargetDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask _targetLayer;

        [SerializeField] private float _detectionRadius = 10f;
        [SerializeField] private float _searchInterval = 0.1f;

        private readonly Collider[] _hitColliders = new Collider[20];

        private float _nextSearchTime;

        public event Action<IDamageable> TargetDetected;
        public IDamageable CurrentTarget { get; private set; }

        private void Update()
        {
            if (Time.time >= _nextSearchTime)
            {
                _nextSearchTime = Time.time + _searchInterval;
                GetClosestTarget();
            }
        }

        private void GetClosestTarget()
        {
            Vector3 myPos = transform.position;

            int targetsCount = Physics.OverlapSphereNonAlloc(myPos, _detectionRadius, _hitColliders, _targetLayer);

            if (targetsCount == 0) return;

            Collider closestCollider = null;
            float minDistanceSqr = float.MaxValue;

            for (int i = 0; i < targetsCount; i++)
            {
                Collider target = _hitColliders[i];

                if (target == null) continue;

                float distanceSqr = (target.transform.position - myPos).sqrMagnitude;

                if (distanceSqr < minDistanceSqr)
                {
                    minDistanceSqr = distanceSqr;
                    closestCollider = target;
                }
            }

            if (closestCollider != null)
            {
                var damageable = closestCollider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    Debug.Log($"[NonAlloc] Ближайший враг: {closestCollider.name}");
                    TargetDetected?.Invoke(damageable);
                    return;
                }
            }

            return;
        }
    }
}
