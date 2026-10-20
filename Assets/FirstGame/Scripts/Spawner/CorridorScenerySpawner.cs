using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class CorridorScenerySpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private Transform[] _sceneryPrefabs;

        [Header("Layout")]
        [SerializeField] private float _minDistanceFromFence = 10f;
        [SerializeField] private float _maxDistanceFromFence = 20f;
        [SerializeField] private float _segmentLength = 40f;
        [SerializeField] private float _corridorLength = 120f;
        [SerializeField] private float _sceneryYOffset = 0f;

        private readonly List<Transform> _sceneryObjects = new List<Transform>();
        private float _fixedX;
        private float _corridorWidth;
        private float _totalLength;

        public void Build(float fixedX, float startZ, float corridorWidth)
        {
            if (_sceneryObjects.Count > 0) return;

            _fixedX = fixedX;
            _corridorWidth = corridorWidth;

            int count = Mathf.CeilToInt(_corridorLength / _segmentLength);
            _totalLength = count * _segmentLength;

            for (int i = 0; i < count; i++)
            {
                float z = startZ + i * _segmentLength;

                float leftX = _fixedX - (_corridorWidth * 0.5f) - Random.Range(_minDistanceFromFence, _maxDistanceFromFence);
                float rightX = _fixedX + (_corridorWidth * 0.5f) + Random.Range(_minDistanceFromFence, _maxDistanceFromFence);

                _sceneryObjects.Add(CreateScenery(new Vector3(leftX, _sceneryYOffset, z)));
                _sceneryObjects.Add(CreateScenery(new Vector3(rightX, _sceneryYOffset, z)));
            }
        }

        public void UpdateSegments(float maxPlayerZ, float backOffset)
        {
            float minZ = maxPlayerZ - backOffset;

            for (int i = 0; i < _sceneryObjects.Count; i++)
            {
                if (_sceneryObjects[i].position.z < minZ)
                {
                    Vector3 pos = _sceneryObjects[i].position;
                    pos.z += _totalLength;

                    bool isLeft = pos.x < _fixedX;
                    float distance = (_corridorWidth * 0.5f) + Random.Range(_minDistanceFromFence, _maxDistanceFromFence);

                    pos.x = isLeft ? _fixedX - distance : _fixedX + distance;

                    _sceneryObjects[i].position = pos;
                    _sceneryObjects[i].rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                }
            }
        }

        private Transform CreateScenery(Vector3 pos)
        {
            Transform prefab = _sceneryPrefabs[Random.Range(0, _sceneryPrefabs.Length)];
            Transform obj = Instantiate(prefab, transform);
            obj.position = pos;
            obj.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            return obj;
        }
    }
}
