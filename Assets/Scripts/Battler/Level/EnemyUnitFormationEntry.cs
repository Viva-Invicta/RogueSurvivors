using UnityEngine;
using Sirenix.OdinInspector;

namespace AutoBattler
{
    [System.Serializable]
    public class EnemyUnitFormationEntry
    {
        [HorizontalGroup(nameof(EnemyUnitFormationEntry), 60)]
        [PreviewField(60)]
        [HideLabel]
        public GameObject Prefab;

        [HorizontalGroup(nameof(EnemyUnitFormationEntry))]
        [LabelWidth(60)]
        public int GridX;

        [HorizontalGroup(nameof(EnemyUnitFormationEntry))]
        [LabelWidth(60)]
        public int GridY;
    }
}