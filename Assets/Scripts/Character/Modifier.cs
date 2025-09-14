namespace DunDungeons
{
    public class Modifier
    {
        private readonly ModifierType type;
        private readonly Faction faction;

        public float Value { get; set; } = 1;

        public ModifierType Type => type;

        public Faction Faction => faction;

        public Modifier(ModifierType type, Faction faction)
        {
            this.type = type;
            this.faction = faction;
        }
    }

    public enum ModifierType
    {
        MovementSpeed,
        AttackSpeed,
        Damage,
        MaxHealth
    }
}