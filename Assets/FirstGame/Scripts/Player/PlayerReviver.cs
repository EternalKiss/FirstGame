using FirstGame.Combat;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace FirstGame.Players
{
    public class PlayerReviver : MonoBehaviour
    {
        [SerializeField] private float _reviveHealthPercent = 0.5f;
        [SerializeField] private int _maxRevives = 1;

        private Health _health;
        private PlayerInputReader _inputReader;
        private PlayerDamageReceiver _damageReceiver;

        private int _reviveCount;

        public bool CanRevive => _reviveCount < _maxRevives;
        public int ReviveCount => _reviveCount;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _inputReader = GetComponent<PlayerInputReader>();
            _damageReceiver = GetComponent<PlayerDamageReceiver>();
        }

        public bool TryRevive()
        {
            if (CanRevive == false) return false;
            if (_health == null) return false;

            _reviveCount++;

            float reviveHealth = _health.MaxHealth * _reviveHealthPercent;
            _health.Revive(reviveHealth);

            if (_inputReader != null)
            {
                _inputReader.enabled = true;
            }

            return true;
        }
    }
}
