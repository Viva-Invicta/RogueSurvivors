using UnityEngine;

namespace DunDungeons
{
    [CreateAssetMenu(menuName = "Configuration/" + nameof(ModifierUpgradeConfiguration))]
    public class ModifierUpgradeConfiguration : ScriptableObject
    {
        [field: SerializeField] public ModifierType ModifierType { get; private set; }
        [field: SerializeField] public ModifierUpgradeType UpgradeType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Title { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
    }

    public enum ModifierUpgradeType
    {
        Multiply,
        Sum
    }
}