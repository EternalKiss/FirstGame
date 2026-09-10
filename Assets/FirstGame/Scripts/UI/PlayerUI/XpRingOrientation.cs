using UnityEngine;

namespace FirstGame.PlayerUI
{
    public class XpRingOrientation : MonoBehaviour
    {
        [SerializeField] private Vector3 _fixedRotation = new Vector3(90f, 0f, 0f);

        private void LateUpdate()
        {
            transform.rotation = Quaternion.Euler(_fixedRotation);
        }
    }
}
