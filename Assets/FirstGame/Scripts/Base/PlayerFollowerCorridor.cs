using UnityEngine;

namespace FirstGame.Base
{
    public class PlayerFollowerCorridor : MonoBehaviour
    {
        [Header("Walls")]
        [SerializeField] private Transform _leftWall;
        [SerializeField] private Transform _rightWall;
        [SerializeField] private Transform _backWall;

        [Header("Settings")]
        [SerializeField] private float _corridorWidth = 12f;
        [SerializeField] private float _corridorLength = 40f;
        [SerializeField] private float _backOffset = 5f;
        [SerializeField] private float _wallHeight = 3f;
        [SerializeField] private float _wallThickness = 0.3f;

        private Transform _player;

        private float _currentZ;
        private float _fixedX;

        public void Initialize(Transform playerTransform)
        {
            _player = playerTransform;
            _fixedX = _player.position.x;
            _currentZ = _player.position.z;

            UpdateWalls();
        }

        private void LateUpdate()
        {
            if (_player == null) return;

            if (_player.position.z > _currentZ)
            {
                _currentZ = _player.position.z;
                UpdateWalls();
            }
        }

        private void UpdateWalls()
        {
            if (_player == null) return;

            Vector3 centerPos = new Vector3(_fixedX, 0f, _currentZ);

            if (_backWall != null)
            {
                _backWall.position = centerPos - (Vector3.forward * _backOffset);
                _backWall.rotation = Quaternion.identity;
                _backWall.localScale = new Vector3(_corridorWidth, _wallHeight, _wallThickness);
            }

            Vector3 corridorCenter = centerPos + Vector3.forward * (_corridorLength * 0.5f - _backOffset);

            if (_leftWall != null)
            {
                _leftWall.position = corridorCenter - (Vector3.right * (_corridorWidth * 0.5f));
                _leftWall.rotation = Quaternion.identity;
                _leftWall.localScale = new Vector3(_wallThickness, _wallHeight, _corridorLength);
            }

            if (_rightWall != null)
            {
                _rightWall.position = corridorCenter + (Vector3.right * (_corridorWidth * 0.5f));
                _rightWall.rotation = Quaternion.identity;
                _rightWall.localScale = new Vector3(_wallThickness, _wallHeight, _corridorLength);
            }
        }
    }
}
