using UnityEngine;

namespace DunDungeons
{
    [CreateAssetMenu(menuName = "Configuration/" + nameof(UpgradesConfiguration))]

    public class UpgradesConfiguration : ScriptableObject
    {
        [field: SerializeField]
        public ModifierUpgradeConfiguration[] ModifierUpgrades { get; private set; }
    }
}