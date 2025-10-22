using UnityEngine;

namespace AutoBattler
{
    [CreateAssetMenu (fileName = nameof(LevelData), menuName = "Configuration/" + nameof(LevelConfiguration))]
    public class LevelConfiguration : ScriptableObject
    {
        [field: SerializeField] public LevelData LevelData { get; private set; }
    }
}