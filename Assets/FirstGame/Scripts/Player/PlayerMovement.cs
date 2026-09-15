using FirstGame.Interfaces;
using UnityEngine;

namespace FirstGame.Players
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;

        private IMovable _movable;
        private IRotatable _rotatable;
        private AnimationController _animationController;
        private float _baseSpeed;

        public float MoveSpeed => _moveSpeed;
        public float BaseMoveSpeed => _baseSpeed;

        private void Awake()
        {
            _baseSpeed = _moveSpeed;
        }

        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = speed;
        }

        public void Initialize(AnimationController animator)
        {
            _movable = GetComponent<IMovable>();
            _rotatable = GetComponent<IRotatable>();
            _animationController = animator;
        }

        public void Move(Vector2 input)
        {
            Vector3 moveDirection = new Vector3(input.x, 0f, input.y);
            Vector3 finalVelocity = Vector3.ClampMagnitude(moveDirection, 1f) * _moveSpeed;

            _movable.Move(finalVelocity);

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                _rotatable.Rotate(moveDirection.normalized);
            }

            bool isMoving = input.sqrMagnitude > 0.01f;
            _animationController.SetIsRunning(isMoving);
        }
    }
}
