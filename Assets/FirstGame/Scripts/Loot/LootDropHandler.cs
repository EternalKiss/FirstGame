using FirstGame.Environment;
using FirstGame.Spawner;
using UnityEngine;

namespace FirstGame.Loot
{
    public class LootDropHandler : MonoBehaviour
    {
        [SerializeField] private StoneSpawner _stoneSpawner;
        [SerializeField] private TreeSpawner _treeSpawner;

        [SerializeField] private LootSpawner _stoneLootSpawner;
        [SerializeField] private LootSpawner _treeLootSpawner;

        public void Initialize()
        {
            if (_stoneSpawner != null) _stoneSpawner.ObjectSpawned += HandleStoneSpawned;
            if (_treeSpawner != null) _treeSpawner.ObjectSpawned += HandleTreeSpawned;
        }

        private void HandleStoneSpawned(Resource stone)
        {
            stone.OnReadyToRelease += HandleStoneDestroyed;
        }

        private void HandleTreeSpawned(Resource tree)
        {
            tree.OnReadyToRelease += HandleTreeDestroyed;
        }

        private void HandleStoneDestroyed(IDestructible destructible)
        {
            destructible.OnReadyToRelease -= HandleStoneDestroyed;

            if (_stoneLootSpawner != null && destructible is Component comp)
            {
                _stoneLootSpawner.SpawnLootExplosion(comp.transform.position);
            }
        }

        private void HandleTreeDestroyed(IDestructible destructible)
        {
            destructible.OnReadyToRelease -= HandleTreeDestroyed;

            if (_treeLootSpawner != null && destructible is Component comp)
            {
                _treeLootSpawner.SpawnLootExplosion(comp.transform.position);
            }
        }

        private void OnDestroy()
        {
            if (_stoneSpawner != null) _stoneSpawner.ObjectSpawned -= HandleStoneSpawned;
            if (_treeSpawner != null) _treeSpawner.ObjectSpawned -= HandleTreeSpawned;
        }
    }
}