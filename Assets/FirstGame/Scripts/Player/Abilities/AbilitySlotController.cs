using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class AbilitySlotController : MonoBehaviour
    {
        [SerializeField] private int _maxSlots = 4;

        private readonly List<AbilityBase> _unlockedAbilities = new List<AbilityBase>(4);

        private AbilityController _abilityController;

        public IReadOnlyList<AbilityBase> UnlockedAbilities => _unlockedAbilities;
        public int MaxSlots => _maxSlots;
        public int UsedSlots => _unlockedAbilities.Count;
        public bool HasFreeSlot => _unlockedAbilities.Count < _maxSlots;

        private void Awake()
        {
            _abilityController = GetComponent<AbilityController>();
        }

        public bool TryUnlock(AbilityBase ability)
        {
            if (ability == null)
            {
                return false;
            }

            if (_unlockedAbilities.Contains(ability))
            {
                return false;
            }

            if (HasFreeSlot == false)
            {
                return false;
            }

            ability.Unlock();
            _unlockedAbilities.Add(ability);
            return true;
        }

        public bool IsUnlocked(AbilityBase ability)
        {
            return _unlockedAbilities.Contains(ability);
        }

        public List<AbilityBase> GetLockedAbilities()
        {
            List<AbilityBase> locked = new List<AbilityBase>();

            if (_abilityController == null)
            {
                return locked;
            }

            var allAbilities = _abilityController.Abilities;

            for (int index = 0; index < allAbilities.Count; index++)
            {
                AbilityBase ability = allAbilities[index];

                if (_unlockedAbilities.Contains(ability))
                {
                    continue;
                }

                locked.Add(ability);
            }

            return locked;
        }
    }
}
