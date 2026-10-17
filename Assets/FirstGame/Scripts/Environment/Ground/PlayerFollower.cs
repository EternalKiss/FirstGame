using UnityEngine;

namespace FirstGame.Environment.Ground
{
    public class PlayerFollower : MonoBehaviour
    {
        [SerializeField] private float _tileSize = 10f;
        [SerializeField] private Vector3 _offset = new Vector3(0f, -0.01f, 0f);

        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void LateUpdate()
        {
            if (_target == null)
            {
                return;
            }

            Vector3 targetPosition = _target.position;

            float snappedX = Mathf.Floor(targetPosition.x / _tileSize) * _tileSize;
            float snappedZ = Mathf.Floor(targetPosition.z / _tileSize) * _tileSize;

            transform.position = new Vector3(snappedX, 0f, snappedZ) + _offset;
        }
    }
}
