using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class GridSpawnManager : MonoBehaviour
    {
        [Header("Настройки сетки карты")]
        [SerializeField] private Vector2 _mapSize = new Vector2(30f, 30f);
        [SerializeField] private float _cellSize = 2.5f;

        [Header("Зона безопасности базы (Центр сцены)")]
        [SerializeField] private Vector3 _baseCenter = Vector3.zero;
        [SerializeField] private float _baseBufferRadius = 6f;

        [SerializeField] private StoneSpawner _stoneSpawner;
        [SerializeField] private TreeSpawner _treeSpawner;

        private List<Vector3> _availablePoints = new List<Vector3>();

        public void GenerateLevel()
        {
            GenerateGridPoints();

            if (_stoneSpawner != null)
            {
                _stoneSpawner.InitializePool();
                SpawnResourcesFromGrid(_stoneSpawner);
            }

            if (_treeSpawner != null)
            {
                _treeSpawner.InitializePool();
                SpawnResourcesFromGrid(_treeSpawner);
            }
        }

        private void GenerateGridPoints()
        {
            _availablePoints.Clear();

            int cellsX = Mathf.FloorToInt(_mapSize.x / _cellSize);
            int cellsZ = Mathf.FloorToInt(_mapSize.y / _cellSize);

            float startX = -_mapSize.x / 2f + _cellSize / 2f;
            float startZ = -_mapSize.y / 2f + _cellSize / 2f;

            for (int x = 0; x < cellsX; x++)
            {
                for (int z = 0; z < cellsZ; z++)
                {
                    float posX = startX + (x * _cellSize);
                    float posZ = startZ + (z * _cellSize);
                    Vector3 potentialPoint = new Vector3(posX, 0f, posZ);

                    float distanceToBase = Vector3.Distance(potentialPoint, _baseCenter);
                    if (distanceToBase > _baseBufferRadius)
                    {
                        _availablePoints.Add(potentialPoint);
                    }
                }
            }

            ShufflePoints(_availablePoints);
        }

        private void SpawnResourcesFromGrid<T>(BaseSpawner<T> spawner) where T : Component, IDestructible
        {
            int countToSpawn = GetSpawnerCount(spawner);

            for (int i = 0; i < countToSpawn; i++)
            {
                if (_availablePoints.Count == 0)
                {
                    Debug.LogWarning("[GridSpawnManager] Карта полностью заполнена! Не хватило свободных ячеек.");
                    break;
                }

                Vector3 spawnPosition = _availablePoints[0];
                _availablePoints.RemoveAt(0);

                float offsetRange = _cellSize * 0.15f;
                float offsetX = Random.Range(-offsetRange, offsetRange);
                float offsetZ = Random.Range(-offsetRange, offsetRange);
                spawnPosition.x += offsetX;
                spawnPosition.z += offsetZ;

                spawner.SpawnAtPosition(spawnPosition);
            }
        }

        private int GetSpawnerCount<T>(BaseSpawner<T> spawner) where T : Component, IDestructible
        {
            if (spawner is StoneSpawner stoneSpawner)
            {
                return stoneSpawner.StonesCount;
            }
            if (spawner is TreeSpawner treeSpawner)
            {
                return treeSpawner.TreesCount;
            }
            return 0;
        }

        private void ShufflePoints(List<Vector3> points)
        {
            for (int i = points.Count - 1; i > 0; i--)
            {
                int rnd = Random.Range(0, i + 1);
                Vector3 temp = points[i];
                points[i] = points[rnd];
                points[rnd] = temp;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(_baseCenter, new Vector3(_mapSize.x, 0.1f, _mapSize.y));

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_baseCenter, _baseBufferRadius);
        }
    }
}