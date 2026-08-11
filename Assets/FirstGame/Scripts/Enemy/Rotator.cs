using UnityEngine;

namespace FirstGame.Enemy
{
    public class Rotator : MonoBehaviour
    {
        public void Rotate(Vector3 target)
        {
            transform.LookAt(target);
        }
    }
}