using FirstGame.Loot;
using FirstGame.ObjectPool;
using FirstGame.Players;
using FirstGame.Players.Inventore;
using FirstGame.Spawner;
using FirstGame.PlayerUI;
using UnityEngine;
using System.Threading.Tasks;

namespace FirstGame.LevelManager
{
    public class Starter : MonoBehaviour
    {
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private BaseSpawner _baseSpawner;
        [SerializeField] private SpawnEnemyTimer _startDelayTimer;
        [SerializeField] private GridSpawnManager _gridSpawnManager;
        [SerializeField] private StoneLootPiecePool _stoneLootPool;
        [SerializeField] private TreeLootPiecePool _treeLootPool;
        [SerializeField] private LootDropHandler _lootDropHandler;
        [SerializeField] private ResourceCounter _resourceCounter;
        [SerializeField] private LevelEndHandler _endLevelHandler;

        private async void Start()
        {
            await LoadLevelAsync();
        }

        private async Task LoadLevelAsync()
        {
            _lootDropHandler?.Initialize();
            _stoneLootPool?.Initialize();
            _treeLootPool?.Initialize();

            await Task.Yield();

            _enemySpawner?.InitializePool();

            Player spawnedPlayer = null;

            if (_playerSpawner != null)
            {
                spawnedPlayer = _playerSpawner.Spawn();
            }

            if (spawnedPlayer != null)
            {
                if (_resourceCounter != null)
                {
                    Inventory playerInventory = spawnedPlayer.GetComponent<Inventory>();
                    _resourceCounter.Initialize(playerInventory);
                }

                _enemySpawner?.SetPlayerTarget(spawnedPlayer.transform);

                if (_baseSpawner != null)
                {
                    _baseSpawner.Initialize(spawnedPlayer.transform, _gridSpawnManager);
                    _endLevelHandler?.Initialize(_baseSpawner);
                }
            }
        }

        private void OnDestroy()
        {
            if (_startDelayTimer != null && _enemySpawner != null)
            {
                _startDelayTimer.OnTimerFinished -= _enemySpawner.StartSpawning;
            }
        }
    }
}
