using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Faction targetFaction;
        [SerializeField] private int damage;
        [SerializeField] private HashSet<HealthComponent> hitTargets = new HashSet<HealthComponent>();

        private bool isActive;

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive)
            {
                return;
            }

            if (!other.TryGetComponent<IHaveFaction>(out var entityWithFaction))
            {
                return;
            }

            if (!other.TryGetComponent<HealthComponent>(out var healthComponent) || hitTargets.Contains(healthComponent))
            {
                return;
            }

            if (entityWithFaction.Faction != targetFaction)
            {
                return;
            }

            hitTargets.Add(healthComponent);
            healthComponent.ConsumeHP(damage);
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