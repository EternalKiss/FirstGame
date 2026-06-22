using UnityEngine;

public class Mover : MonoBehaviour, IMovable
{
    private CharacterController _controller;
    private float _verticalVelocity;

    private float _groundedGravity = -2f;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void Move(Vector3 motionVector)
    {
        if (_controller.isGrounded)
        {
            _verticalVelocity = _groundedGravity;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        motionVector.y = _verticalVelocity;
        _controller.Move(motionVector * Time.deltaTime);
    }
}
