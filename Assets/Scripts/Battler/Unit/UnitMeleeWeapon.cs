using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitMeleeWeapon : MonoBehaviour
    {
        [SerializeField] private HashSet<UnitBehaviourController> hitTargets = new HashSet<UnitBehaviourController>();

        private bool isActive;
        private UnitBehaviourController owner;
        private DamageType damageType;

        public void Initialize(UnitBehaviourController owner, DamageType damageType)
        {
            this.owner = owner;
            this.damageType = damageType;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
            {
                return;
            }

            if (!other.TryGetComponent<UnitBehaviourController>(out var unit))
            {
                return;
            }

            if (hitTargets.Contains(unit))
            {
                return;
            }

            var otherUnitStatusProvider = unit.StatusProvider;

            var isValidTarget =
               otherUnitStatusProvider.State == UnitState.Fight &&
               otherUnitStatusProvider.Faction != owner.StatusProvider.Faction;

            if (!isValidTarget)
            {
                return;
            }

            var ownerStatusProvider = owner.StatusProvider;

            var damage = ownerStatusProvider.UnitValuesCalculator.CalculateOutcomingDamage(damageType);
            unit.RecieveDamage(damageType, damage);
            
            hitTargets.Add(unit);
        }

        public void Activate()
        {
            isActive = true;
            hitTargets.Clear();
        }

        public void Deactivate()
        {
            isActive = false;
        }
    }
}