using UnityEngine;

public class TargetDetectorSphere : MonoBehaviour
{
    [SerializeField] private LayerMask _layersForDetect;

    private float _detectionRadius = 5f;
    private float _searchInterval = 0.1f;
    private float _nextSearchTime;

    private Collider[] _targets = new Collider[10]; 

    public IDamageable CurrentTarget { get; private set; }

    private void Update()
    {
        if (Time.time >= _nextSearchTime)
        {
            _nextSearchTime = Time.time + _searchInterval;
            CurrentTarget = GetClosestTarget();
        }
    }

    private IDamageable GetClosestTarget()
    {
        int targetsCount = Physics.OverlapSphereNonAlloc(transform.position, _detectionRadius, _targets, _layersForDetect);

        if (targetsCount == 0) return null;

        Collider closestCollider = null;
        float minDistanceSqr = float.MaxValue;
        Vector3 myPos = transform.position;

        for (int i = 0; i < targetsCount; i++)
        {
            Collider target = _targets[i];

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
                return damageable;
            }
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
