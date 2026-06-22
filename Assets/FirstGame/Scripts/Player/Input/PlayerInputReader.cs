using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    private Player _player;

    private PlayerInput _playerInput;

    private Vector2 _inputDirection;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        _player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void Update()
    {
        _inputDirection = _playerInput.Game.Move.ReadValue<Vector2>();
        _player.Move(_inputDirection);
    }
}
