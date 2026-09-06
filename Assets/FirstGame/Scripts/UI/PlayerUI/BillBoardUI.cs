using UnityEngine;

namespace FirstGame.PlayerUI
{
    public class BillboardUI : MonoBehaviour
    {
        private Transform _mainCameraTransform;

        private void Awake()
        {
            if (UnityEngine.Camera.main != null)
            {
                _mainCameraTransform = UnityEngine.Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (_mainCameraTransform != null)
            {
                transform.LookAt(transform.position + _mainCameraTransform.rotation * Vector3.forward, _mainCameraTransform.rotation * Vector3.up);
            }
        }
    }
}
