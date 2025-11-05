namespace AutoBattler
{
    public class UnitStatus : IUnitStatusProvider, IUnitStatusSetter
    {
        public UnitStatus(UnitFaction faction, UnitConfiguration configuration)
        {
            Faction = faction;
        }

        public UnitConfiguration Configuration { get; set; }
        public UnitFaction Faction { get; set; }

        public UnitStateID StateID { get; set; }
        public Resource Health { get; set; }

        public bool IsMovementLocked { get; set; }
        public bool IsAttackLocked { get; set; }

        public bool IsAttackInCooldown { get; set; }

        public UnitValuesCalculator UnitValuesCalculator { get; set; }

        public UnitWeapon Weapon { get; set; }

        public (int x, int y) GridPosition { get; set; }
    }

    public interface IUnitStatusProvider
    {
        public UnitConfiguration Configuration { get; }
        public UnitFaction Faction { get; }

        public UnitStateID StateID { get; }

        public Resource Health { get; }

        public bool IsMovementLocked { get; }
        public bool IsAttackLocked { get; }

        public bool IsAttackInCooldown { get; }

        public UnitValuesCalculator UnitValuesCalculator { get; }
        public UnitWeapon Weapon { get; }

        public (int x, int y) GridPosition { get; }
    }

    public interface IUnitStatusSetter
    {
        public UnitConfiguration Configuration { set; }
        public UnitFaction Faction { set; }

        public UnitStateID StateID { set; }

        public Resource Health { set; }

        public bool IsMovementLocked { set; }
        public bool IsAttackLocked { set; }

        public bool IsAttackInCooldown { set; }

        public UnitValuesCalculator UnitValuesCalculator { set; }

        public UnitWeapon Weapon { set; }

        public (int x, int y) GridPosition { set; }
    }
}