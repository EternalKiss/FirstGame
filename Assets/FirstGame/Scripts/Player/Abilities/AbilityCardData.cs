namespace FirstGame.Players.Abilities
{
    public readonly struct AbilityCardData
    {
        public readonly AbilityBase Ability;
        public readonly bool IsNewAbility;

        public AbilityCardData(AbilityBase ability, bool isNewAbility)
        {
            Ability = ability;
            IsNewAbility = isNewAbility;
        }
    }
}