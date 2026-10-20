using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Spawner
{
    public class CorridorFenceSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _fencePrefab;
        [SerializeField] private float _fenceSegmentLength = 2f;
        [SerializeField] private float _corridorLength = 60f;

        private readonly List<Transform> _leftFences = new List<Transform>();
        private readonly List<Transform> _rightFences = new List<Transform>();
        private readonly List<Transform> _backFences = new List<Transform>();

        private float _fixedX;
        private float _corridorWidth;
        private float _totalLength;

        public void Build(float fixedX, float startZ, float corridorWidth)
        {
            if (_leftFences.Count > 0) return;

            _fixedX = fixedX;
            _corridorWidth = corridorWidth;

            int count = Mathf.CeilToInt(_corridorLength / _fenceSegmentLength);
            _totalLength = count * _fenceSegmentLength;

            for (int i = 0; i < count; i++)
            {
                float z = startZ + i * _fenceSegmentLength;

                Transform left = Instantiate(_fencePrefab, transform);
                left.position = new Vector3(_fixedX - _corridorWidth * 0.5f, 0f, z);
                left.rotation = Quaternion.Euler(0, 90, 0);
                _leftFences.Add(left);

                Transform right = Instantiate(_fencePrefab, transform);
                right.position = new Vector3(_fixedX + _corridorWidth * 0.5f, 0f, z);
                right.rotation = Quaternion.Euler(0, -90, 0);
                _rightFences.Add(right);
            }

            int backCount = Mathf.CeilToInt(_corridorWidth / _fenceSegmentLength);
            for (int i = 0; i < backCount; i++)
            {
                float x = fixedX + (i - (backCount - 1) * 0.5f) * _fenceSegmentLength;
                Transform back = Instantiate(_fencePrefab, transform);
                back.position = new Vector3(x, 0f, startZ);
                back.rotation = Quaternion.identity;
                _backFences.Add(back);
            }
        }

        public void UpdateSegments(float maxPlayerZ, float backOffset)
        {
            float minZ = maxPlayerZ - backOffset;

            for (int i = 0; i < _leftFences.Count; i++)
            {
                if (_leftFences[i].position.z < minZ)
                {
                    Vector3 leftPos = _leftFences[i].position;
                    leftPos.z += _totalLength;
                    _leftFences[i].position = leftPos;

                    Vector3 rightPos = _rightFences[i].position;
                    rightPos.z += _totalLength;
                    _rightFences[i].position = rightPos;
                }
            }

            foreach (var back in _backFences)
            {
                Vector3 pos = back.position;
                pos.z = minZ;
                back.position = pos;
            }
        }
    }
}
