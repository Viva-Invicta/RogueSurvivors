using System.Collections.Generic;

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

        public UnitState State { get; set; }
        public Resource Health { get; set; }

        public bool IsMovementLocked { get; set; }
        public bool IsAttackLocked { get; set; }

        public bool IsAttackInCooldown { get; set; }

        public UnitValuesCalculator UnitValuesCalculator { get; set; }

        public UnitWeapon Weapon { get; set; }
    }

    public interface IUnitStatusProvider
    {
        public UnitConfiguration Configuration { get; set; }
        public UnitFaction Faction { get; }

        public UnitState State { get; }

        public Resource Health { get; }

        public bool IsMovementLocked { get; }
        public bool IsAttackLocked { get; }

        public bool IsAttackInCooldown { get; }

        public UnitValuesCalculator UnitValuesCalculator { get; }
        public UnitWeapon Weapon { get; }
    }

    public interface IUnitStatusSetter
    {
        public UnitConfiguration Configuration { get; set; }
        public UnitFaction Faction { set; }

        public UnitState State { set; }

        public Resource Health { set; }

        public bool IsMovementLocked { set; }
        public bool IsAttackLocked { set; }

        public bool IsAttackInCooldown { set; }

        public UnitValuesCalculator UnitValuesCalculator { set; }

        public UnitWeapon Weapon { set; }

    }
}