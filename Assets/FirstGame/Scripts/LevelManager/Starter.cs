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
        [SerializeField] private GridSpawnManager _gridSpawnManager;
        [SerializeField] private StoneLootPiecePool _stoneLootPool;
        [SerializeField] private TreeLootPiecePool _treeLootPool;
        [SerializeField] private LootDropHandler _lootDropHandler;
        [SerializeField] private ResourceCounter _resourceCounter;
        [SerializeField] private LevelEndHandler _endLevelHandler;
        [SerializeField] private LevelProgressController _progressController;
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private float _initialCorridorLength = 30f;

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
                    Vector3 initialBasePos = spawnedPlayer.transform.position + Vector3.forward * _initialCorridorLength;
                    var initialBase = _baseSpawner.SpawnBase(initialBasePos);

                    if (_gridSpawnManager != null)
                    {
                        _gridSpawnManager.GenerateLevel(initialBase.EntrancePosition, Vector3.back, _initialCorridorLength);
                    }

                    if (_progressController != null)
                    {
                        _progressController.Initialize(_baseSpawner, _gridSpawnManager);
                        _progressController.SetInitialBase(initialBase);
                        _endLevelHandler?.Initialize(_progressController);
                    }

                    _dayNightCycle?.StartDay();
                }
            }
        }
    }
}
