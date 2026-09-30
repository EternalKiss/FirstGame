using FirstGame.Common;
using FirstGame.Spawner;
using System;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class GameCycleController : MonoBehaviour
    {
        [SerializeField] private BossSpawner _bossSpawner;
        [SerializeField] private LevelProgressController _progressController;
        [SerializeField] private float _cycleMultiplier = 0.5f;
        [SerializeField] private float _bossDistanceSpawn = 20f;

        private UniversalEventBinder _eventBinder = new UniversalEventBinder();
        private Transform _playerTransform;
        private float _cycleCount = 1;

        public float CycleCount => _cycleCount;

        public event Action<float> CycleStarted;

        private void OnEnable()
        {
            _eventBinder.Bind(
                callback => _progressController.LevelCompleted += callback,
                callback => _progressController.LevelCompleted -= callback,
                HandleLevelCompleted);

            _eventBinder.Bind(
                callback => _bossSpawner.BossDied += callback,
                callback => _bossSpawner.BossDied -= callback,
                HandleBossDied);
        }

        private void OnDestroy()
        {
            _eventBinder.UnbindAll();
        }

        public void SetPlayerPosition(Transform playerPosition)
        {
            _playerTransform = playerPosition;
        }

        private void HandleLevelCompleted()
        {
            SpawnBoss();
        }

        private void SpawnBoss()
        {
            if(_bossSpawner != null)
            {
                float multiplier = 1f + (_cycleCount - 1) * _cycleMultiplier;
                Vector3 spawnPosition = _playerTransform.position + Vector3.forward * _bossDistanceSpawn;

                _bossSpawner.Spawn(multiplier, spawnPosition);
            }
        }

        private void HandleBossDied()
        {
            _cycleCount++;

            if (_progressController != null)
            {
                _progressController.StartNewCycle();
            }

            CycleStarted?.Invoke(_cycleCount);
        }
    }
}