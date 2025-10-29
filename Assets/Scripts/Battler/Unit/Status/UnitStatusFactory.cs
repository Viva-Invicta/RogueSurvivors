using System.Collections.Generic;

namespace AutoBattler
{
    public class UnitStatusFactory
    {
        public UnitStatus Create(UnitFaction faction, UnitConfiguration config)
        {
            var unitStatus = new UnitStatus(faction);

            var unitValuesCalculator = new UnitValuesCalculator(unitStatus);
            unitStatus.UnitValuesCalculator = unitValuesCalculator;

            var unitHealth = new Resource(config.BaseMaxHealth);
            unitStatus.Health = unitHealth;

            var baseDamageDictionary = new Dictionary<DamageType, float>();
            foreach (var baseDamageEntry in config.BaseDamage)
            {
                baseDamageDictionary.Add(baseDamageEntry.DamageType, baseDamageEntry.Value);
            }

            unitStatus.BaseDamage = baseDamageDictionary;
            unitStatus.BaseMovementSpeed = config.BaseMovementSpeed;
            unitStatus.BaseAttackCooldown = config.BaseAttackCooldown;

            return unitStatus;
        }
    }
}