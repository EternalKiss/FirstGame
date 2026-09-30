using UnityEngine;
using FirstGame.Players;
using FirstGame.Players.Input;

public class PlayerInputReader : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

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

    public void SetJoystick(Joystick joystick)
    {
        _joystick = joystick;
    }

    private void Update()
    {
        Vector2 direction = _playerInput.Game.Move.ReadValue<Vector2>();

        if (_joystick != null && _joystick.Direction.sqrMagnitude > 0.01f)
        {
            direction = _joystick.Direction;
        }

        _inputDirection = direction;
        _player.Move(_inputDirection);
    }
}
