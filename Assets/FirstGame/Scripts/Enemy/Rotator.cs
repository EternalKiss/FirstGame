using UnityEngine;

namespace FirstGame.Enemy
{
    public class Rotator : MonoBehaviour
    {
        public void Rotate(Vector3 target)
        {
            Vector3 lockedTarget = new Vector3(target.x, transform.position.y, target.z);

            transform.LookAt(lockedTarget);
        }
    }
}