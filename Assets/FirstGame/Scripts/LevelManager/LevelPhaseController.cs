using FirstGame.Spawner;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class LevelPhaseController : MonoBehaviour
    {
        [SerializeField] private EnemySpawner _enemySpawner;

        private BaseSpawner _baseSpawner;

        private void OnDestroy()
        {
            if (_baseSpawner != null)
            {
                _baseSpawner.PlayerEnteredBase -= CleanCurrentStage;
                _baseSpawner.PlayerLeftBase -= StartEnemySpawning;
            }
        }

        public void Initialize(BaseSpawner baseSpawner)
        {
            _baseSpawner = baseSpawner;
            _baseSpawner.PlayerEnteredBase += CleanCurrentStage;
            _baseSpawner.PlayerLeftBase += StartEnemySpawning;
        }

        private void CleanCurrentStage()
        {
            if (_enemySpawner != null)
            {
                _enemySpawner.ClearActiveEnemies();
            }
        }

        private void StartEnemySpawning()
        {
            if (_enemySpawner != null)
            {
                _enemySpawner.StartSpawning();
            }
        }
    }
}
