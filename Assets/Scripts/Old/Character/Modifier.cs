using System;

namespace DunDungeons
{
    public class Modifier
    {
        public event Action Updated;

        private readonly ModifierType type;
        private readonly Faction faction;

        private float currentValue = 1;

        public float Value
        {
            get 
            {
                return currentValue; 
            }
            set
            {
                currentValue = value;
                Updated?.Invoke();
            }
        }

        public ModifierType Type => type;

        public Faction Faction => faction;

        public Modifier(ModifierType type, Faction faction)
        {
            this.type = type;
            this.faction = faction;
        }

        public static float operator *(float left, Modifier right)
        {
            return left * right.Value;
        }
    }

    public enum ModifierType
    {
        MovementSpeed,
        AttackCooldown,
        Damage,
        MaxHealth,
        DashCooldown,
        WeaponSize
    }
}