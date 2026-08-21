using UnityEngine;
using FirstGame.Loot;

namespace FirstGame.Players
{
    public class LootMagnetSystem : MonoBehaviour
    {
        [SerializeField] private LayerMask _lootLayer;
        [SerializeField] private float _magnetRadius = 5f;
        [SerializeField] private float _magnetSpeed = 14f;
        [SerializeField] private float _rotateSpeed = 120f;

        private readonly Collider[] _hitColliders = new Collider[50];
        private Transform _playerTransform;

        private void Awake()
        {
            _playerTransform = transform;
        }

        private void Update()
        {
            Vector3 playerPos = _playerTransform.position;
            int lootCount = Physics.OverlapSphereNonAlloc(playerPos, _magnetRadius, _hitColliders, _lootLayer);

            for (int i = 0; i < lootCount; i++)
            {
                Collider col = _hitColliders[i];
                if (col == null) continue;

                if (col.TryGetComponent(out LootPiece loot))
                {
                    if (!loot.IsCollectable) continue;

                    if (!loot.IsMagnetized)
                    {
                        loot.Magnetize();
                    }

                    Vector3 targetPos = playerPos + Vector3.up * 0.5f;
                    loot.transform.position = Vector3.MoveTowards(loot.transform.position, targetPos, _magnetSpeed * Time.deltaTime);
                    loot.transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime, Space.World);

                    float distance = Vector3.Distance(loot.transform.position, targetPos);
                    if (distance < 0.4f)
                    {
                        loot.Collect();
                    }
                }
            }
        }
    }
}
