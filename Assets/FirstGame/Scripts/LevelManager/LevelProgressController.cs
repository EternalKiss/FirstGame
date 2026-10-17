using FirstGame.Base;
using FirstGame.Spawner;
using System;
using UnityEngine;

namespace FirstGame.LevelManager
{
    public class LevelProgressController : MonoBehaviour
    {
        [SerializeField] private int _maxBasesPerLevel = 3;
        [SerializeField] private GridSpawnManager _gridSpawnManager;
        [SerializeField] private float _corridorLength = 30f;

        private BaseSpawner _baseSpawner;

        private PlayerBase _currentActiveBase;
        private PlayerBase _previousBase;
        private int _basesEnteredCount = 0;
        private bool _isFinalBaseEntered;

        private Vector3 _lastExitPosition;
        private bool _hasExitPosition;

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

        public void SetInitialBase(PlayerBase initialBase, Vector3 startPosition)
        {
            _currentActiveBase = initialBase;
            _baseSpawner.BindBase(_currentActiveBase);

            GenerateResources(initialBase);
        }

        public void StartNewCycle()
        {
            _basesEnteredCount = 0;
            _previousBase = null;
            _isFinalBaseEntered = false;

            if (_hasExitPosition == false) return;

            Vector3 nextSpawnPos = BasePositionCalculator.CalculatePlacementPosition(
                _lastExitPosition,
                _corridorLength,
                _baseSpawner.BaseEntranceOffset
            );

            _currentActiveBase = _baseSpawner.SpawnBase(nextSpawnPos);
            _baseSpawner.BindBase(_currentActiveBase);

            GenerateResources(_currentActiveBase);
        }

        private void HandleBaseEntered()
        {
            _basesEnteredCount++;

            if (_basesEnteredCount >= _maxBasesPerLevel)
            {
                _isFinalBaseEntered = true;
                return;
            }

            _previousBase = _currentActiveBase;

            Vector3 nextSpawnPos = BasePositionCalculator.CalculatePlacementPosition(
                _previousBase.ExitPosition,
                _corridorLength,
                _baseSpawner.BaseEntranceOffset
            );

            _currentActiveBase = _baseSpawner.SpawnBase(nextSpawnPos);
            _baseSpawner.BindBase(_currentActiveBase);

            GenerateResources(_currentActiveBase);
        }

        private void HandleBaseExited()
        {
            if (_isFinalBaseEntered)
            {
                _isFinalBaseEntered = false;

                if (_currentActiveBase != null)
                {
                    _lastExitPosition = _currentActiveBase.ExitPosition;
                    _hasExitPosition = true;

                    _baseSpawner.UnbindBase(_currentActiveBase);
                    _baseSpawner.DestroyBase(_currentActiveBase);
                    _currentActiveBase = null;
                }

                LevelCompleted?.Invoke();
                return;
            }

            if (_previousBase != null)
            {
                _baseSpawner.DestroyBase(_previousBase);
                _previousBase = null;
            }
        }

        private void GenerateResources(PlayerBase targetBase)
        {
            if (_gridSpawnManager != null)
            {
                _gridSpawnManager.GenerateLevel(
                    targetBase.EntrancePosition,
                    Vector3.back,
                    _corridorLength
                );
            }
        }
    }
}