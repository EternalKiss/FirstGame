using System.Collections.Generic;
using UnityEngine;
using FirstGame.Common;

namespace FirstGame.Players.Abilities
{
    [CreateAssetMenu(fileName = "AbilityDatabase", menuName = "FirstGame/Ability Database")]
    public class AbilityDatabase : ScriptableObject
    {
        [SerializeField] private Sprite _defaultIcon;
        [SerializeField] private string _defaultDescription = "Новая способность";

        public Sprite DefaultIcon => _defaultIcon;
        public string DefaultDescription => _defaultDescription;

        public List<AbilityCardData> GetRandomCards(int count, AbilitySlotController slotController)
        {
            List<AbilityCardData> result = new List<AbilityCardData>(count);

            if (slotController == null)
            {
                return result;
            }

            List<AbilityBase> locked = slotController.GetLockedAbilities();

            ListShuffler.Shuffle(locked);

            int cardsToGive = Mathf.Min(count, locked.Count);

            for (int index = 0; index < cardsToGive; index++)
            {
                result.Add(new AbilityCardData(locked[index], true));
            }

            return result;
        }
    }
}
