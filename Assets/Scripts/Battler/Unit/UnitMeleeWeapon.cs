using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitMeleeWeapon : UnitWeapon
    {
        [SerializeField] private HashSet<UnitBehaviourController> hitTargets = new HashSet<UnitBehaviourController>();

        private bool isActive;
        private UnitBehaviourController owner;
        private IEnumerable<DamageType> damageTypes;

        public override void Initialize(UnitBehaviourController owner, IEnumerable<DamageType> damageType)
        {
            this.owner = owner;
            damageTypes = damageType;
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
            var ownerStatusProvider = owner.StatusProvider;

            var isValidTarget =
               unit.StateMachine.CurrentStateID == UnitStateID.Fight &&
               otherUnitStatusProvider.Faction != owner.StatusProvider.Faction;

            if (!isValidTarget)
            {
                return;
            }

            var damageReciever = unit.ComponentsContainer.DamageReceiver;
            var ownerValuesCalculator = ownerStatusProvider.UnitValuesCalculator;

            foreach (var damageType in damageTypes)
            {
                var damage = ownerValuesCalculator.CalculateOutcomingDamage(damageType);
                damageReciever.ReceiveDamage(damageType, damage);
            }

            hitTargets.Add(unit);
        }

        public override void Activate()
        {
            isActive = true;
            hitTargets.Clear();
        }

        public override void Deactivate()
        {
            isActive = false;
        }
    }
}