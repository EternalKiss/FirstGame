using FirstGame.Base;
using FirstGame.Spawner;
using System;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class LevelProgressController : MonoBehaviour
    {
        [SerializeField] private int _maxBasesPerLevel = 3;
        [SerializeField] private float _resourceCorridorLength = 30f;

        private BaseSpawner _baseSpawner;
        private GridSpawnManager _gridSpawnManager;

        private PlayerBase _currentActiveBase;
        private PlayerBase _previousBase;
        private int _basesEnteredCount = 0;

        public event Action LevelCompleted;

        private void OnDestroy()
        {
            if (_baseSpawner != null)
            {
                _baseSpawner.PlayerEnteredBase -= HandleBaseEntered;
                _baseSpawner.PlayerLeftBase -= HandleBaseExited;
            }
        }

        public void Initialize(BaseSpawner baseSpawner, GridSpawnManager gridSpawnManager)
        {
            _baseSpawner = baseSpawner;
            _gridSpawnManager = gridSpawnManager;

            _baseSpawner.PlayerEnteredBase += HandleBaseEntered;
            _baseSpawner.PlayerLeftBase += HandleBaseExited;
        }

        public void SetInitialBase(PlayerBase initialBase)
        {
            _currentActiveBase = initialBase;
            _baseSpawner.BindBase(_currentActiveBase);
        }

        private void HandleBaseEntered()
        {
            _basesEnteredCount++;

            if (_basesEnteredCount >= _maxBasesPerLevel)
            {
                if (_currentActiveBase != null) _baseSpawner.UnbindBase(_currentActiveBase);
                if (_previousBase != null) _baseSpawner.UnbindBase(_previousBase);

                LevelCompleted?.Invoke();
                return;
            }

            _previousBase = _currentActiveBase;

            Vector3 nextSpawnPos = BasePositionCalculator.CalculatePlacementPosition(
                _previousBase.ExitPosition,
                _resourceCorridorLength,
                _baseSpawner.BaseEntranceOffset
            );

            _currentActiveBase = _baseSpawner.SpawnBase(nextSpawnPos);
            _baseSpawner.BindBase(_currentActiveBase);

            if (_gridSpawnManager != null)
            {
                _gridSpawnManager.GenerateLevel(_currentActiveBase.EntrancePosition, Vector3.back, _resourceCorridorLength);
            }
        }

        private void HandleBaseExited()
        {
            if (_basesEnteredCount > 1 && _previousBase != null)
            {
                _baseSpawner.DestroyBase(_previousBase);
                _previousBase = null;
            }
        }
    }
}
