using FirstGame.Common;
using FirstGame.Spawner;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class DayNightEnemyBinder : MonoBehaviour
    {
        [SerializeField] private DayNightCycle _dayNightCycle;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private BaseSpawner _baseSpawner;

        private readonly UniversalEventBinder _eventBinder = new UniversalEventBinder();

        private void OnEnable()
        {
            _eventBinder.Bind(
                callback => _dayNightCycle.NightStarted += callback,
                callback => _dayNightCycle.NightStarted -= callback,
                HandleNightStarted);

            _eventBinder.Bind(
                callback => _dayNightCycle.SunriseStarted += callback,
                callback => _dayNightCycle.SunriseStarted -= callback,
                HandleSunriseStarted);

            _eventBinder.Bind(
                callback => _baseSpawner.PlayerEnteredBase += callback,
                callback => _baseSpawner.PlayerEnteredBase -= callback,
                HandlePlayerEnteredBase);

            _eventBinder.Bind(
                callback => _baseSpawner.PlayerLeftBase += callback,
                callback => _baseSpawner.PlayerLeftBase -= callback,
                HandlePlayerLeftBase);
        }

        private void OnDisable()
        {
            _eventBinder.UnbindAll();
        }

        private void HandleNightStarted()
        {
            _enemySpawner.StartSpawning();
        }

        private void HandleSunriseStarted()
        {
            _enemySpawner.ClearActiveEnemies();
        }

        private void HandlePlayerEnteredBase()
        {
            _dayNightCycle.Stop();
            _enemySpawner.ClearActiveEnemies();
        }

        private void HandlePlayerLeftBase()
        {
            _dayNightCycle.StartDay();
        }
    }
}
