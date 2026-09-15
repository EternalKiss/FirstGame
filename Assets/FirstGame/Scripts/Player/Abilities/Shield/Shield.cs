using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class Shield : AbilityBase
    {
        [SerializeField] private GameObject _shieldVisual;

        private PlayerDamageReceiver _damageReceiver;

        private void Awake()
        {
            _damageReceiver = GetComponent<PlayerDamageReceiver>();
        }

        private void OnDisable()
        {
            if (_shieldVisual != null)
            {
                _shieldVisual.SetActive(false);
            }

            if (_damageReceiver != null)
            {
                _damageReceiver.SetShielded(false);
            }
        }

        protected override void Activate()
        {
            if (_shieldVisual != null)
            {
                _shieldVisual.SetActive(true);
            }

            if (_damageReceiver != null)
            {
                _damageReceiver.SetShielded(true);
            }
        }

        protected override void Deactivate()
        {
            if (_shieldVisual != null)
            {
                _shieldVisual.SetActive(false);
            }

            if (_damageReceiver != null)
            {
                _damageReceiver.SetShielded(false);
            }
        }

        public override bool HasTargets()
        {
            return true;
        }
    }
}
