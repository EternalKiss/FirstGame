using FirstGame.Interfaces;
using UnityEngine;

namespace FirstGame.Players
{
    public class Rotator : MonoBehaviour, IRotatable
    {
        private float _rotationSpeed = 720f;

        public void Rotate(Vector3 direction)
        {
            if (direction.sqrMagnitude < 0.001f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime
            );
        }
    }
}
