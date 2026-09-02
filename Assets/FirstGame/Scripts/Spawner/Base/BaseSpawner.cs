using FirstGame.Base;
using System;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class BaseSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerBase _basePrefab;
        [SerializeField] private float _resourceCorridorLength = 30f;
        [SerializeField] private float _spawnHeight = 0.1f;

        [SerializeField] private int _maxBasesPerLevel = 3;

        private GridSpawnManager _gridSpawnManager;
        private Transform _playerTransform;
        private Vector3 _lastSpawnPosition;
        private PlayerBase _currentActiveBase;
        private PlayerBase _previousBase;
        private BaseTriggerBinder _triggerBinder;
        private int _basesEnteredCount = 0;

        public event Action LevelCompleted;

        private void Awake()
        {
            _triggerBinder = new BaseTriggerBinder(HandlePlayerExited, HandlePlayerEntered);
        }

        private void OnDestroy()
        {
            if (_triggerBinder != null)
            {
                if (_currentActiveBase != null)
                {
                    _triggerBinder.Unbind(_currentActiveBase);
                }

                if (_previousBase != null)
                {
                    _triggerBinder.Unbind(_previousBase);
                }
            }
        }

        public void Initialize(Transform playerTransform, GridSpawnManager gridSpawnManager)
        {
            _playerTransform = playerTransform;
            _gridSpawnManager = gridSpawnManager;

            Vector3 spawnPos = _playerTransform.position + Vector3.forward * _resourceCorridorLength;
            spawnPos.y = _spawnHeight;

            _currentActiveBase = Instantiate(_basePrefab, spawnPos, Quaternion.identity);

            if (_gridSpawnManager != null)
            {
                _gridSpawnManager.GenerateLevel(_currentActiveBase.EntrancePosition, Vector3.back, _resourceCorridorLength);
            }

            _triggerBinder.Bind(_currentActiveBase);
        }

        private void HandlePlayerEntered()
        {
            _basesEnteredCount++;

            if (_gridSpawnManager != null)
            {
                _gridSpawnManager.ClearResourcesOnly();
            }

            if (_basesEnteredCount >= _maxBasesPerLevel)
            {
                _triggerBinder.Unbind(_currentActiveBase);

                if (LevelCompleted != null)
                {
                    LevelCompleted.Invoke();
                }
                return;
            }

            SpawnNextStage();
        }

        private void HandlePlayerExited(Vector3 playerPosition)
        {
            if (_basesEnteredCount > 1 && _previousBase != null)
            {
                _triggerBinder.Unbind(_previousBase);
                Destroy(_previousBase.gameObject);
                _previousBase = null;
            }
        }

        private void SpawnNextStage()
        {
            _triggerBinder.Unbind(_currentActiveBase);
            _previousBase = _currentActiveBase;

            Vector3 localEntranceOffset = _basePrefab.EntrancePosition - _basePrefab.transform.position;

            Vector3 spawnPos = BasePositionCalculator.CalculatePlacementPosition(
                _previousBase.ExitPosition,
                _resourceCorridorLength,
                localEntranceOffset
            );
            spawnPos.y = _spawnHeight;

            _currentActiveBase = Instantiate(_basePrefab, spawnPos, Quaternion.identity);

            if (_gridSpawnManager != null)
            {
                _gridSpawnManager.GenerateLevel(_currentActiveBase.EntrancePosition, Vector3.back, _resourceCorridorLength);
            }

            _triggerBinder.Bind(_currentActiveBase);
        }
    }
}
