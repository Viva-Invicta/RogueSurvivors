using UnityEngine;
using Sirenix.OdinInspector;

namespace AutoBattler
{
    [System.Serializable]
    public class EnemyUnitFormationEntry
    {
        [VerticalGroup("Unit")]
        [HideLabel]
        [SerializeField]
        private UnitType unitType;

        [HorizontalGroup("X")]
        [HideLabel]
        [SerializeField]
        private int gridX;

        [HorizontalGroup("Y")]
        [HideLabel]
        [SerializeField]
        private int gridY;

        public UnitType UnitType => unitType;
        public int GridX => gridX;
        public int GridY => gridY;
    }
}