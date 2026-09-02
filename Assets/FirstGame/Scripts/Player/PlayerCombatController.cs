using FirstGame.Combat;
using FirstGame.Interfaces;
using System;
using UnityEngine;

namespace FirstGame.Players
{
    public class PlayerCombatController : MonoBehaviour
    {
        public event Action<Component> OnAttackStarted;
        public event Action OnAttackStopped;

        private TargetDetector _targetDetector;
        private AnimationController _animationController;
        private DamageDealer _damageDealer;
        private IRotatable _rotatable;
        private float _damage;
        private bool _isAttacking;

        private void Update()
        {
            if (_isAttacking && _damageDealer.CanAttack)
            {
                _isAttacking = false;
            }

            if (_isAttacking || (_targetDetector.TargetsInRange != null && _targetDetector.TargetsInRange.Count > 0))
            {
                RotateTowardsClosestTarget();
            }

            HandleCombatTick();
        }

        public void Initialize(TargetDetector targetDetector, AnimationController animationController, DamageDealer damageDealer, float attackInterval, float damage)
        {
            _targetDetector = targetDetector;
            _animationController = animationController;
            _damageDealer = damageDealer;
            _damage = damage;

            _rotatable = GetComponent<IRotatable>();

            _damageDealer.InitializeCooldown(attackInterval);

            _animationController.SynchronizeAnimationSpeed(attackInterval, AnimationController.AttackTrigger);
            _animationController.AddAttackEventViaCode(AnimationController.AttackTrigger, AnimationController.AttackEventMethodName, 0.55f);
        }

        private void RotateTowardsClosestTarget()
        {
            var targets = _targetDetector.TargetsInRange;
            if (targets == null || targets.Count == 0) return;

            if (targets[0] is Component targetComp && targetComp != null)
            {
                Vector3 directionToTarget = targetComp.transform.position - transform.position;
                directionToTarget.y = 0;

                if (directionToTarget.sqrMagnitude > 0.01f)
                {
                    _rotatable.Rotate(directionToTarget.normalized);
                }
            }
        }

        public void OnAttackHitEvent()
        {
            var targets = _targetDetector.TargetsInRange;
            int targetsCount = targets.Count;

            for (int i = 0; i < targetsCount; i++)
            {
                IDamageable target = targets[i];
                if (target is UnityEngine.Object unityObj && unityObj != null)
                {
                    target.TakeDamage(_damage);
                }
            }
        }

        public void InterruptAttack()
        {
            _isAttacking = false;
            _animationController.ResetAttackTrigger();
            _damageDealer.StopCooldown();

            if (_targetDetector.TargetsInRange == null || _targetDetector.TargetsInRange.Count == 0)
            {
                if (OnAttackStopped != null)
                {
                    OnAttackStopped.Invoke();
                }
            }
        }

        private void HandleCombatTick()
        {
            if (_isAttacking)
            {
                return;
            }

            var targets = _targetDetector.TargetsInRange;

            if (targets != null && targets.Count > 0)
            {
                IDamageable primaryTarget = targets[0];

                if (_damageDealer.Attack(primaryTarget, _damage))
                {
                    _isAttacking = true;

                    if (primaryTarget is Component targetComp)
                    {
                        if (OnAttackStarted != null)
                        {
                            OnAttackStarted.Invoke(targetComp);
                        }
                    }

                    _animationController.Attack();
                }
            }
            else
            {
                if (OnAttackStopped != null)
                {
                    OnAttackStopped.Invoke();
                }
            }
        }
    }
}
