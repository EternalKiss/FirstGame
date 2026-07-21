using FirstGame.Players;
using FirstGame.Spawner;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class Starter : MonoBehaviour
    {
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;

        private void Awake()
        {
            Player spawnedPlayer = _playerSpawner.Spawn();

            if (_enemySpawner != null && spawnedPlayer != null)
            {
                _enemySpawner.SetPlayerTarget(spawnedPlayer);
            }
            else
            {
                Debug.LogError("[LevelStarter] Не удалось связать игрока со спавнером врагов!");
            }
        }
    }
}
