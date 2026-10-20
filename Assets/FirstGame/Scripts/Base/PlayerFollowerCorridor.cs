using FirstGame.Spawner;
using UnityEngine;

namespace FirstGame.Base
{
    public class PlayerFollowerCorridor : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private CorridorFenceSpawner _fenceSpawner;
        [SerializeField] private CorridorScenerySpawner _scenerySpawner;

        [Header("Settings")]
        [SerializeField] private float _corridorWidth = 12f;
        [SerializeField] private float _backOffset = 10f;
        [SerializeField] private float _snapLength = 2f;

        private float _fixedX;
        private float _maxPlayerZ;
        private float _lastSnappedZ;
        private bool _isInitialized;

        public void Initialize(Transform playerTransform)
        {
            if (_isInitialized) return;

            _player = playerTransform;
            _fixedX = _player.position.x;

            _maxPlayerZ = _player.position.z;
            _lastSnappedZ = Mathf.Floor(_maxPlayerZ / _snapLength) * _snapLength;

            float startZ = _maxPlayerZ - _backOffset;

            if (_fenceSpawner != null) _fenceSpawner.Build(_fixedX, startZ, _corridorWidth);
            if (_scenerySpawner != null) _scenerySpawner.Build(_fixedX, startZ, _corridorWidth);

            UpdateAllSegments();

            _isInitialized = true;
        }

        private void LateUpdate()
        {
            if (_player == null || !_isInitialized) return;

            if (_player.position.z > _maxPlayerZ)
            {
                _maxPlayerZ = _player.position.z;
            }

            float currentSnappedZ = Mathf.Floor(_maxPlayerZ / _snapLength) * _snapLength;

            if (currentSnappedZ > _lastSnappedZ)
            {
                _lastSnappedZ = currentSnappedZ;
                UpdateAllSegments();
            }
        }

        private void UpdateAllSegments()
        {
            if (_fenceSpawner != null) _fenceSpawner.UpdateSegments(_lastSnappedZ, _backOffset);
            if (_scenerySpawner != null) _scenerySpawner.UpdateSegments(_lastSnappedZ, _backOffset);
        }
    }
}
