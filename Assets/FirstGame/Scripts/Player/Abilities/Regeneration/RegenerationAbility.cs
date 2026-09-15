using FirstGame.Combat;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class RegenerationAbility : AbilityBase
    {
        [SerializeField] private float _healAmount = 5f;
        [SerializeField] private ParticleSystem _healVfx;

        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        protected override void Activate()
        {
            if (_health == null)
            {
                return;
            }

            _health.Heal(_healAmount);

            if (_healVfx != null)
            {
                _healVfx.Play();
            }
        }
    }
}
