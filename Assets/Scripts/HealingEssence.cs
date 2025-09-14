using UnityEngine;

namespace DunDungeons
{
    public class HealingEssence : PickupItem
    {
        [SerializeField] private int healAmount;

        protected override bool CheckPickupConditions(Collider other)
        {
            return other.GetComponent<HealthComponent>();
        }

        protected override void OnAfterPickup(Collider other)
        {
            if (!other.TryGetComponent<HealthComponent>(out var healthComponent))
            {
                return;
            }

            healthComponent.AddHP(healAmount);
        }
    }
}