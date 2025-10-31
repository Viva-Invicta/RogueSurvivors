using UnityEngine;

namespace AutoBattler
{
    public class UnitDamageReceiver : MonoBehaviour, IInitializableWithUnitStatusComponent
    {
        private IUnitStatusProvider statusProvider;

        public void Initialize(IUnitStatusProvider unitStatus)
        {
            statusProvider = unitStatus;
        }

        public void ReceiveDamage(DamageType damageType, float value)
        {
            var damage = statusProvider.UnitValuesCalculator.CalculateIncomingDamage(value, damageType);
            statusProvider.Health.Consume(damage);
        }
    }
}