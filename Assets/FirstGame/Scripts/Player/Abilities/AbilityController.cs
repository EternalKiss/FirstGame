using System.Collections.Generic;
using UnityEngine;

namespace FirstGame.Players.Abilities
{
    public class AbilityController : MonoBehaviour
    {
        private readonly List<AbilityBase> _abilities = new List<AbilityBase>(8);

        public IReadOnlyList<AbilityBase> Abilities => _abilities;

        private void Awake()
        {
            AbilityBase[] foundAbilities = GetComponents<AbilityBase>();

            for (int index = 0; index < foundAbilities.Length; index++)
            {
                _abilities.Add(foundAbilities[index]);
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            for (int index = 0; index < _abilities.Count; index++)
            {
                AbilityBase ability = _abilities[index];
                ability.Tick(deltaTime);

                if (ability.Trigger == AbilityTrigger.Interval)
                {
                    ability.TryActivate();
                }
            }
        }

        public void TriggerAbilities(AbilityTrigger trigger)
        {
            for (int index = 0; index < _abilities.Count; index++)
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