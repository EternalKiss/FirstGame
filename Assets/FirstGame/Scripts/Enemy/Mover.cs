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
            Vector3 offset = targetPosition - transform.position;
            offset.y = 0f;

            float sqrDistance = offset.sqrMagnitude;

            if (sqrDistance <= attackDistance * attackDistance)
            {
                Stop();
                _targetReached = true;
                return;
            }

            Vector3 direction = offset.normalized;
            Vector3 newPosition = transform.position + direction * _speed * Time.deltaTime;

            _rigidBody.MovePosition(newPosition);
            _targetReached = false;
        }

        public void Stop()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }
    }
}
