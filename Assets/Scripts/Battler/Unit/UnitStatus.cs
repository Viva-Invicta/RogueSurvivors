namespace AutoBattler
{
    public class UnitStatus : IUnitStatusProvider, IUnitStatusSetter
    {
        public UnitStatus(UnitFaction faction)
        {
            Faction = faction;
        }

        public UnitFaction Faction { get; set; }

        public bool IsMovementLocked { get; set; }
        public bool IsAttackLocked { get; set; }

        public bool IsWeaponInCooldown { get; set; }

        public float MovementSpeed { get; set; }
    }

    public interface IUnitStatusProvider
    {
        public UnitFaction Faction { get; }

        public bool IsMovementLocked { get; }
        public bool IsAttackLocked { get; }

        public bool IsWeaponInCooldown { get; }

        public float MovementSpeed { get; }
    }

    public interface IUnitStatusSetter
    {
        public UnitFaction Faction { set; }

        public bool IsMovementLocked { set; }
        public bool IsAttackLocked { set; }

        public bool IsWeaponInCooldown { set; }

        public float MovementSpeed { set; }
    }
}