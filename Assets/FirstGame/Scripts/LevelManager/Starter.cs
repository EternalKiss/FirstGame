using FirstGame.Players;
using FirstGame.Spawner;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class Starter : MonoBehaviour
    {
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private ResourceSpawner _resourceSpawner;
        [SerializeField] private SpawnEnemyTimer _startDelayTimer;

        private void Awake()
        {
            _enemySpawner.InitializePool();
            _resourceSpawner.InitializePool();

            Player spawnedPlayer = _playerSpawner.Spawn();
            _enemySpawner.SetPlayerTarget(spawnedPlayer.transform);
            _resourceSpawner.SpawnResources();

            _startDelayTimer.OnTimerFinished += _enemySpawner.StartSpawning;

            _startDelayTimer.StartTimer();
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
