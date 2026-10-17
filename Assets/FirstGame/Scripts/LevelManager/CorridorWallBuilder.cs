using UnityEngine;

namespace FirstGame.LevelManager
{
    public class CorridorWallBuilder : MonoBehaviour
    {
        [SerializeField] private Transform _leftWall;
        [SerializeField] private Transform _rightWall;
        [SerializeField] private Transform _backWall;
        [SerializeField] private float _wallHeight = 3f;
        [SerializeField] private float _wallThickness = 0.3f;
        [SerializeField] private float _backOffset = 3f;

        public void BuildSideWalls(Vector3 startPosition, Vector3 direction, float length, float width)
        {
            if (_leftWall == null || _rightWall == null)
            {
                return;
            }

            Vector3 actualStart = startPosition - direction * _backOffset;
            float actualLength = length + _backOffset;

            Vector3 right = new Vector3(direction.z, 0f, -direction.x);
            float halfWidth = width * 0.5f;
            Vector3 centerOffset = direction * (actualLength * 0.5f);

            Quaternion forwardRotation = Quaternion.LookRotation(direction);
            Vector3 sideScale = new Vector3(_wallThickness, _wallHeight, actualLength);

            _leftWall.gameObject.SetActive(true);
            _leftWall.position = actualStart + (-right * halfWidth) + centerOffset;
            _leftWall.rotation = forwardRotation;
            _leftWall.localScale = sideScale;

            _rightWall.gameObject.SetActive(true);
            _rightWall.position = actualStart + (right * halfWidth) + centerOffset;
            _rightWall.rotation = forwardRotation;
            _rightWall.localScale = sideScale;
        }

        public void ShowBackWallAt(Vector3 frontEdgePosition, Vector3 direction, float width)
        {
            if (_backWall == null)
            {
                return;
            }

            Vector3 actualPosition = frontEdgePosition - direction * _backOffset;

            Vector3 right = new Vector3(direction.z, 0f, -direction.x);
            Quaternion sideRotation = Quaternion.LookRotation(right);
            Vector3 backScale = new Vector3(_wallThickness, _wallHeight, width);

            _backWall.gameObject.SetActive(true);
            _backWall.position = actualPosition;
            _backWall.rotation = sideRotation;
            _backWall.localScale = backScale;
        }

        public void HideBackWall()
        {
            if (_backWall != null)
            {
                _backWall.gameObject.SetActive(false);
            }
        }

        public void HideWalls()
        {
            if (_leftWall != null) _leftWall.gameObject.SetActive(false);
            if (_rightWall != null) _rightWall.gameObject.SetActive(false);
            if (_backWall != null) _backWall.gameObject.SetActive(false);
        }
    }
}