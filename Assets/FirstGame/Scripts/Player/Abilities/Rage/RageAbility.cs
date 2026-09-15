using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class RageAbility : AbilityBase
    {
        [SerializeField] private float _attackSpeedMultiplier = 1.5f;

        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        protected override void Activate()
        {
            if (_player == null)
            {
                return;
            }

            _player.SetAttackSpeedMultiplier(_attackSpeedMultiplier);
        }

        protected override void Deactivate()
        {
            if (_player == null)
            {
                return;
            }

            _player.SetAttackSpeedMultiplier(1f);
        }

        public override bool HasTargets()
        {
            return true;
        }
    }
}