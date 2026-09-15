using FirstGame.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public static class AbilityTargetFinder
    {
        public static int FindTargetsInRadius(Vector3 position, float radius, LayerMask layer, Collider[] buffer, List<IDamageable> results)
        {
            results.Clear();

            int hitCount = Physics.OverlapSphereNonAlloc(position, radius, buffer, layer);

            for (int index = 0; index < hitCount; index++)
            {
                Collider hitCollider = buffer[index];

                if (hitCollider == null) continue;
                if (hitCollider.TryGetComponent(out IDamageable damageable) == false) continue;
                if (damageable.IsAlive == false) continue;

                results.Add(damageable);
            }

            return results.Count;
        }

        public static IDamageable FindClosestTarget(Vector3 position, float radius, LayerMask layer, Collider[] buffer, HashSet<Component> exclude)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(position, radius, buffer, layer);

            IDamageable closest = null;
            float closestSqr = float.MaxValue;

            for (int index = 0; index < hitCount; index++)
            {
                Collider hitCollider = buffer[index];

                if (hitCollider == null) continue;
                if (hitCollider.TryGetComponent(out IDamageable damageable) == false) continue;
                if (damageable.IsAlive == false) continue;
                if (damageable is not Component component) continue;
                if (exclude != null && exclude.Contains(component)) continue;

                Vector3 offset = component.transform.position - position;
                offset.y = 0f;
                float sqr = offset.sqrMagnitude;

                if (sqr < closestSqr)
                {
                    closestSqr = sqr;
                    closest = damageable;
                }
            }

            return closest;
        }
    }
}
