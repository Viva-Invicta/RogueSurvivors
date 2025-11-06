using System.Collections.Generic;
using System.Linq;

namespace AutoBattler
{
    public class UnitStatusFactory
    {
        public UnitStatus Create(UnitFaction faction, UnitConfiguration config, UnitWeapon weapon)
        {
            var unitStatus = new UnitStatus(faction);

            unitStatus.Configuration = config;

            var unitValuesCalculator = new UnitValuesCalculator(unitStatus, config);
            unitStatus.UnitValuesCalculator = unitValuesCalculator;

            var unitHealth = new Resource(config.BaseMaxHealth);
            unitStatus.Health = unitHealth;

            var baseDamageDictionary = new Dictionary<DamageType, float>();
            foreach (var baseDamageEntry in config.BaseDamage)
            {
                baseDamageDictionary.Add(baseDamageEntry.DamageType, baseDamageEntry.Value);
            }

            unitStatus.Weapon = weapon;

            return unitStatus;
        }
    }
}