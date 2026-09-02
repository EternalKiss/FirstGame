using FirstGame.Common;
using FistGame.Spawner;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class GridSpawnManager : MonoBehaviour
    {
        [SerializeField] private float _corridorWidth = 12f;
        [SerializeField] private float _startOffsetFromGate = 2f;
        [SerializeField] private float _playerSafetyRadius = 5f;
        [SerializeField] private float _cellSize = 2.5f;

        [SerializeField] private float _resourceHeightOffset = 0f;

        [SerializeField] private StoneSpawner _stoneSpawner;
        [SerializeField] private TreeSpawner _treeSpawner;

        private readonly List<Vector3> _availablePoints = new List<Vector3>(256);
        private CorridorPointsGenerator _pointsGenerator;
        private bool _isPoolsInitialized = false;

        private void Awake()
        {
            _pointsGenerator = new CorridorPointsGenerator(_corridorWidth, _cellSize, _startOffsetFromGate, 0f);
        }

        public void GenerateLevel(Vector3 entranceGatePosition, Vector3 direction, float corridorLength)
        {
            float usableLength = corridorLength - _startOffsetFromGate - _playerSafetyRadius;
            if (usableLength <= 0f)
            {
                return;
            }

            HandlePoolsLifecycle();

            _pointsGenerator.FillPoints(_availablePoints, entranceGatePosition, direction, usableLength, _resourceHeightOffset);
            ListShuffler.Shuffle(_availablePoints);

            int pointIndex = 0;

            if (_stoneSpawner != null)
            {
                SpawnGroup(_stoneSpawner, ref pointIndex);
            }

            if (_treeSpawner != null)
            {
                SpawnGroup(_treeSpawner, ref pointIndex);
            }
        }

        public void ClearResourcesOnly()
        {
            if (_stoneSpawner != null)
            {
                _stoneSpawner.ClearActiveObjects();
            }

            if (_treeSpawner != null)
            {
                _treeSpawner.ClearActiveObjects();
            }
        }

        private void HandlePoolsLifecycle()
        {
            if (_isPoolsInitialized == false)
            {
                if (_stoneSpawner != null)
                {
                    _stoneSpawner.InitializePool();
                }

                if (_treeSpawner != null)
                {
                    _treeSpawner.InitializePool();
                }

                _isPoolsInitialized = true;
            }
            else
            {
                ClearResourcesOnly();
            }
        }

        private void SpawnGroup<T>(BasisSpawner<T> spawner, ref int pointIndex) where T : Component, IDestructible
        {
            int count = 0;

            if (spawner is StoneSpawner stone)
            {
                count = stone.StonesCount;
            }
            else if (spawner is TreeSpawner tree)
            {
                count = tree.TreesCount;
            }

            for (int i = 0; i < count; i++)
            {
                if (pointIndex >= _availablePoints.Count)
                {
                    break;
                }

                spawner.SpawnAtPosition(_availablePoints[pointIndex]);
                pointIndex++;
            }
        }
    }
}