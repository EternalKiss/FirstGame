using UnityEngine;

namespace FirstGame.Players.Abilities
{
    [CreateAssetMenu(fileName = "AbilityData", menuName = "FirstGame/Ability Data")]
    public class AbilityData : ScriptableObject
    {
        [SerializeField] private string _displayName = "Ability";
        [SerializeField] private string _description = "Описание способностей";
        [SerializeField] private Sprite _icon;

        public string DisplayName => _displayName;
        public string Description => _description;
        public Sprite Icon => _icon;
    }
}
