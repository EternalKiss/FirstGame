using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class AbilityController : MonoBehaviour
    {
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private string _meleeAbilityClipName = "AbilityAttack";
        [SerializeField] private float _hitTimePercent = 0.5f;

        private readonly List<AbilityBase> _abilities = new List<AbilityBase>(8);

        private Player _player;
        private AbilityBase _pendingAbility;
        private float _ignoreCombatEndTime;
        private float _pendingDamageTime;
        private float _abilityAnimationDuration = 0.8f;

        public IReadOnlyList<AbilityBase> Abilities => _abilities;

        private void Awake()
        {
            _player = GetComponent<Player>();

            AbilityBase[] foundAbilities = GetComponents<AbilityBase>();

            for (int index = 0; index < foundAbilities.Length; index++)
            {
                _abilities.Add(foundAbilities[index]);
            }
        }

        private void Start()
        {
            if (_animationController == null)
            {
                return;
            }

            _abilityAnimationDuration = _animationController.GetClipLength(_meleeAbilityClipName);
        }

        private void Update()
        {
            float currentTime = Time.time;
            float deltaTime = Time.deltaTime;

            int abilityCount = _abilities.Count;

            for (int index = 0; index < abilityCount; index++)
            {
                AbilityBase ability = _abilities[index];
                ability.Tick(deltaTime);

                if (ability.Trigger != AbilityTrigger.Interval)
                {
                    continue;
                }

                if (ability.TryActivate() == false)
                {
                    continue;
                }

                if (ability.UsesMeleeAnimation == false)
                {
                    continue;
                }

                _pendingAbility = ability;
                StartAbilityAnimation(currentTime);
            }

            if (_pendingAbility != null && currentTime >= _pendingDamageTime)
            {
                ApplyPendingDamage();
            }

            if (_ignoreCombatEndTime > 0f && currentTime >= _ignoreCombatEndTime)
            {
                _ignoreCombatEndTime = 0f;

                if (_player != null)
                {
                    _player.SetIgnoreCombatAttackEvents(false);
                }
            }
        }

        private void StartAbilityAnimation(float currentTime)
        {
            if (_player != null)
            {
                _player.SetIgnoreCombatAttackEvents(true);
            }

            if (_animationController != null)
            {
                _animationController.PlayMeleeAbility();
            }

            _ignoreCombatEndTime = currentTime + _abilityAnimationDuration;
            _pendingDamageTime = currentTime + _abilityAnimationDuration * _hitTimePercent;
        }

        public void ApplyPendingDamage()
        {
            if (_pendingAbility == null)
            {
                return;
            }

            _pendingAbility.OnApplyDamage();
            _pendingAbility = null;
        }

        public void TriggerAbilities(AbilityTrigger trigger)
        {
            int abilityCount = _abilities.Count;

            for (int index = 0; index < abilityCount; index++)
            {
                AbilityBase ability = _abilities[index];

                if (ability.Trigger == trigger)
                {
                    ability.TryActivate();
                }
            }
        }
    }
}