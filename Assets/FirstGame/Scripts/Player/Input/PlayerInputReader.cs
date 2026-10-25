using FirstGame.GameTutorial;
using FirstGame.Players;
using FirstGame.Players.Input;
using UnityEngine;

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

    private void OnEnable() => _playerInput.Enable();
    private void OnDisable() => _playerInput.Disable();

    public void SetJoystick(Joystick joystick)
    {
        _joystick = joystick;
    }

    private void Update()
    {
        Vector2 keyboardInput = _playerInput.Game.Move.ReadValue<Vector2>();
        Vector2 joystickInput = _joystick != null ? _joystick.Direction : Vector2.zero;

        Vector2 direction = keyboardInput.sqrMagnitude > 0.01f ? keyboardInput : joystickInput;

        _inputDirection = direction;
        _player.Move(_inputDirection);

        if (_inputDirection.sqrMagnitude > 0.01f)
        {
            if (TutorialManager.Instance != null)
            {
                TutorialManager.Instance.OnPlayerMoved();
            }
        }
    }
}
