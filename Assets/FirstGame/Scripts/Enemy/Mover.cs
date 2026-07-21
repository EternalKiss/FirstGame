using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class Mover : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private Rigidbody _rigidBody;

        private bool _targetReached;

        public bool TargetReached => _targetReached;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 targetPosition, float attackDistance)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            direction.y = 0;

            Vector3 newPosition = transform.position + direction * _speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, targetPosition) > attackDistance)
            {
                _rigidBody.MovePosition(newPosition);
                _targetReached = false;
            }
            else
            {
                Stop();
                _targetReached = true;
            }
        }

        public void Stop()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }
    }
}
