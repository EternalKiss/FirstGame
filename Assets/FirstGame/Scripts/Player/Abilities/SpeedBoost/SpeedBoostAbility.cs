using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class SpeedBoostAbility : AbilityBase
    {
        [SerializeField] private float _speedMultiplier = 1.15f;

        private PlayerMovement _playerMovement;

        private void Awake()
        {
            _playerMovement = GetComponent<PlayerMovement>();
        }

        protected override void Activate()
        {
            if (_playerMovement == null)
            {
                return;
            }

            float newSpeed = _playerMovement.BaseMoveSpeed * _speedMultiplier;
            _playerMovement.SetMoveSpeed(newSpeed);
        }
    }
}
