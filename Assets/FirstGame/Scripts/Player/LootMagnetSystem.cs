using UnityEngine;
using FirstGame.Loot;
using FirstGame.Players.Inventore;

namespace FirstGame.Players
{
    public class LootMagnetSystem : MonoBehaviour
    {
        [SerializeField] private LayerMask _lootLayer;
        [SerializeField] private float _magnetRadius = 5f;
        [SerializeField] private float _magnetSpeed = 14f;
        [SerializeField] private float _rotateSpeed = 120f;
        [SerializeField] private float _searchInterval = 0.15f;

        private readonly Collider[] _hitColliders = new Collider[50];
        private Transform _playerTransform;
        private Inventory _inventory;
        private float _nextSearchTime;

        private void Awake()
        {
            _playerTransform = transform;
            _inventory = GetComponent<Inventory>();
        }

        private void Update()
        {
            Vector3 playerPos = _playerTransform.position;
            Vector3 targetPos = playerPos + Vector3.up * 0.5f;

            HandleLootScanning(playerPos);
            HandleLootProcessing(playerPos, targetPos, Time.deltaTime);
        }

        private void HandleLootScanning(Vector3 playerPos)
        {
            if (Time.time < _nextSearchTime) return;

            _nextSearchTime = Time.time + _searchInterval;

            int lootCount = Physics.OverlapSphereNonAlloc(playerPos, _magnetRadius, _hitColliders, _lootLayer);

            for (int i = 0; i < lootCount; i++)
            {
                Collider col = _hitColliders[i];
                if (col == null) continue;

                if (col.TryGetComponent(out LootPiece loot))
                {
                    if (loot.IsCollectable && !loot.IsMagnetized)
                    {
                        loot.Magnetize();
                    }
                }
            }
        }

        private void HandleLootProcessing(Vector3 playerPos, Vector3 targetPos, float deltaTime)
        {
            int activeLootCount = Physics.OverlapSphereNonAlloc(playerPos, _magnetRadius, _hitColliders, _lootLayer);
            for (int i = 0; i < activeLootCount; i++)
            {
                Collider col = _hitColliders[i];
                if (col == null) continue;

                if (col.TryGetComponent(out LootPiece loot))
                {
                    ProcessSingleLoot(loot, playerPos, targetPos, deltaTime);
                }
            }
        }

        private void ProcessSingleLoot(LootPiece loot, Vector3 playerPos, Vector3 targetPos, float deltaTime)
        {
            if (!loot.IsCollectable || !loot.IsMagnetized) return;

            loot.transform.position = Vector3.MoveTowards(loot.transform.position, targetPos, _magnetSpeed * deltaTime);

            if (loot.transform.childCount > 0)
            {
                loot.transform.GetChild(0).Rotate(Vector3.up, _rotateSpeed * deltaTime, Space.World);
            }

            float distance = Vector3.Distance(loot.transform.position, targetPos);

            if (distance < 0.5f)
            {
                CollectLootData(loot, playerPos);
                loot.Collect();
            }
        }

        private void CollectLootData(LootPiece loot, Vector3 playerPos)
        {
            if (_inventory == null) return;

            if (loot.Type == LootType.Stone)
            {
                _inventory.AddStone(1);
                UI.FloatingTextManager.Instance?.SpawnText("+1", playerPos, LootType.Stone);
            }
            else if (loot.Type == LootType.Tree)
            {
                _inventory.AddTree(1);
                UI.FloatingTextManager.Instance?.SpawnText("+1", playerPos, LootType.Tree);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _magnetRadius);
        }
    }
}
