using FirstGame.Interfaces;
using FirstGame.Sound;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class AreaChopAbility : AbilityBase
    {
        [SerializeField] private float _damage = 25f;

        private TargetDetector _targetDetector;

        public override bool UsesMeleeAnimation => true;

        protected override void OnAwake()
        {
            _targetDetector = GetComponent<TargetDetector>();
        }

        public override bool HasTargets()
        {
            if (_targetDetector == null)
            {
                return false;
            }

            return _targetDetector.TargetsInRange.Count > 0;
        }

        protected override void Activate()
        {
        }

        public override void OnApplyDamage()
        {
            if (_targetDetector == null)
            {
                return;
            }

            if (AudioService.Instance != null)
            {
                AudioService.Instance.PlayAreaChop();
            }

            var targets = _targetDetector.TargetsInRange;
            int count = targets.Count;

            for (int index = 0; index < count; index++)
            {
                IDamageable damageable = targets[index];

                if (damageable == null)
                {
                    continue;
                }

                if (damageable.IsAlive == false)
                {
                    continue;
                }

                damageable.TakeDamage(_damage);

                if (index == 0)
                {
                    OnImpact(damageable);
                }
            }
        }

        public override void OnImpact(IDamageable target)
        {
            if (AudioService.Instance != null)
            {
                AudioService.Instance.PlayHitOnTarget(target);
            }
        }
    }
}