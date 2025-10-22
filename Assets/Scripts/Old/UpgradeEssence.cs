using UnityEngine;

namespace DunDungeons
{
    public class UpgradeEssence : PickupItem
    {
        [SerializeField]
        private UpgradesConfiguration upgradesConfiguration;

        protected override void OnAfterPickup(Collider other)
        {
            FindObjectOfType<ServiceLocator>().UIService.ShowUpgradesWindow(upgradesConfiguration);
        }
    }
}