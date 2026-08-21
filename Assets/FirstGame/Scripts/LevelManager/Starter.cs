using FirstGame.ObjectPool;
using FirstGame.Players;
using FirstGame.Spawner;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class Starter : MonoBehaviour
    {
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private SpawnEnemyTimer _startDelayTimer;
        [SerializeField] private GridSpawnManager _gridSpawnManager;
        [SerializeField] private StoneLootPiecePool _stoneLootPool;
        [SerializeField] private TreeLootPiecePool _treeLootPool;

        private void Awake()
        {
            if (_stoneLootPool != null)
            {
                _stoneLootPool.Initialize();
            }

            if (_treeLootPool != null)
            {
                _treeLootPool.Initialize();
            }

            if (_gridSpawnManager != null)
            {
                _gridSpawnManager.GenerateLevel();
            }

            if (_enemySpawner != null)
            {
                _enemySpawner.InitializePool();
            }

            Player spawnedPlayer = null;

            if (_playerSpawner != null)
            {
                spawnedPlayer = _playerSpawner.Spawn();
            }

            if (_enemySpawner != null && spawnedPlayer != null)
            {
                _enemySpawner.SetPlayerTarget(spawnedPlayer.transform);
            }

            if (_startDelayTimer != null && _enemySpawner != null)
            {
                _startDelayTimer.OnTimerFinished += _enemySpawner.StartSpawning;
                _startDelayTimer.StartTimer();
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
