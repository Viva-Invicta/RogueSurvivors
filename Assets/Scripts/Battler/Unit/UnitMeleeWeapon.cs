using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitMeleeWeapon : MonoBehaviour
    {
        [SerializeField] private HashSet<UnitBehaviourController> hitTargets = new HashSet<UnitBehaviourController>();

        private bool isActive;
        private UnitFaction ownerFaction;

        public void Initialize(UnitFaction ownerFaction)
        {
            this.ownerFaction = ownerFaction;
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

            var isValidTarget =
                unit.State == UnitState.Fight &&
                unit.UnitStatusProvider.Faction != ownerFaction;

            if (!isValidTarget)
            {
                return;
            }

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