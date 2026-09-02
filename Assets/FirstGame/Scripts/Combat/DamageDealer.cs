using FirstGame.Core;
using FirstGame.Interfaces;
using UnityEngine;

namespace FirstGame.Combat
{
    public class DamageDealer : MonoBehaviour
    {
        private GameplayTimer _attackCooldown;
        private bool _canAttack = true;

        public bool CanAttack => _canAttack;

        private void Awake()
        {
            _attackCooldown = new GameplayTimer(1f);
            _attackCooldown.OnTimerFinished += ResetCooldown;
        }

        public void InitializeCooldown(float interval)
        {
            if (_attackCooldown != null)
            {
                _attackCooldown.ChangeDuration(interval);
            }
        }

        private void Update()
        {
            _attackCooldown?.Tick(Time.deltaTime);
        }

        public bool Attack(IDamageable target, float damage)
        {
            if (_canAttack)
            {
                _canAttack = false;
                _attackCooldown.Start();
                return true;
            }

            return false;
        }

        public void StopCooldown()
        {
            _attackCooldown?.Stop();
            _canAttack = true;
        }

        private void ResetCooldown()
        {
            _canAttack = true;
        }
    }
}
