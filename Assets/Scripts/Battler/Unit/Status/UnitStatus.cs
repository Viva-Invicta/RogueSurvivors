using System.Collections.Generic;

namespace AutoBattler
{
    public class UnitStatus : IUnitStatusProvider, IUnitStatusSetter
    {
        public UnitStatus(UnitFaction faction)
        {
            Faction = faction;
        }

        public UnitFaction Faction { get; set; }

        public UnitState State { get; set; }
        public Resource Health { get; set; }

        public bool IsMovementLocked { get; set; }
        public bool IsAttackLocked { get; set; }

        public bool IsAttackInCooldown { get; set; }

        public float BaseMovementSpeed { get; set; }
        public float BaseAttackCooldown { get; set; }

        public UnitValuesCalculator UnitValuesCalculator { get; set; }
        public Dictionary<DamageType, float> BaseDamage  { get; set; }
    }

    public interface IUnitStatusProvider
    {
        public UnitFaction Faction { get; }

        public UnitState State { get; }

        public Resource Health { get; }

        public bool IsMovementLocked { get; }
        public bool IsAttackLocked { get; }

        public bool IsAttackInCooldown { get; }

        public float BaseMovementSpeed { get; }
        public float BaseAttackCooldown { get; }

        public UnitValuesCalculator UnitValuesCalculator { get; }
        public Dictionary<DamageType, float> BaseDamage { get; }
    }

    public interface IUnitStatusSetter
    {
        public UnitFaction Faction { set; }

        public UnitState State { set; }

        public Resource Health { set; }

        public bool IsMovementLocked { set; }
        public bool IsAttackLocked { set; }

        public bool IsAttackInCooldown { set; }

        public float BaseAttackCooldown { set; }
        public float BaseMovementSpeed { set; }

        public UnitValuesCalculator UnitValuesCalculator { set; }
        public Dictionary<DamageType, float> BaseDamage { set; }

    }
}