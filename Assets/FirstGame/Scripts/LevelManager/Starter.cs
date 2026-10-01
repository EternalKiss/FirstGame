using FirstGame.Loot;
using FirstGame.ObjectPool;
using FirstGame.Players;
using FirstGame.Players.Abilities;
using FirstGame.Players.Input;
using FirstGame.Players.Inventore;
using FirstGame.Players.Level;
using FirstGame.PlayerUI;
using FirstGame.Sound;
using FirstGame.Spawner;
using System.Threading.Tasks;
using UnityEngine;

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
        [SerializeField] private LevelProgressController _progressController;
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private LevelUpScreen _levelUpScreen;
        [SerializeField] private SellScreen _sellScreen;
        [SerializeField] private UpgradeScreen _upgradeScreen;
        [SerializeField] private GameOverHandler _gameOverHandler;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private GameCycleController _gameCycleController;
        [SerializeField] private AudioEventBridge _audioEventBridge;
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

            if (spawnedPlayer == null)
            {
                return;
            }

            if (_joystick != null)
            {
                PlayerInputReader inputReader = spawnedPlayer.GetComponent<PlayerInputReader>();

                if (inputReader != null)
                {
                    inputReader.SetJoystick(_joystick);
                }
            }

            Inventory inventory = spawnedPlayer.GetComponent<Inventory>();
            Experience experience = spawnedPlayer.GetComponent<Experience>();
            AbilitySlotController slotController = spawnedPlayer.GetComponent<AbilitySlotController>();

            if (_levelUpScreen != null)
            {
                _levelUpScreen.Initialize(experience, slotController);
            }

            if (_resourceCounter != null)
            {
                _resourceCounter.Initialize(inventory);
            }

            if (_sellScreen != null)
            {
                _sellScreen.Initialize(inventory);
            }

            if (_upgradeScreen != null)
            {
                _upgradeScreen.Initialize(inventory, slotController);
            }

            if (_gameOverHandler != null)
            {
                _gameOverHandler.Initialize(spawnedPlayer);
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
                }

                _dayNightCycle?.StartDay();

                if (_audioEventBridge != null)
                {
                    _audioEventBridge.BindToPlayer(spawnedPlayer);
                }

                if (_gameCycleController != null)
                {
                    _gameCycleController.SetPlayerPosition(spawnedPlayer.transform);
                }
            }
        }
    }
}
