using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AutoBattler
{
    [System.Serializable]
    public class WaveFormation
    {
        [MinValue(0)]
        public int WaveOrder = 0;

        [TableList]
        public List<EnemyUnitFormationEntry> Enemies = new();
    }

    [CreateAssetMenu(menuName = "AutoBattler/Enemy Formation Config", fileName = "EnemyFormation")]
    public class EnemyFormationConfig : ScriptableObject
    {
        [field: SerializeField]
        public Vector2Int RoomSize;

        [TableList]
        [ListDrawerSettings(HideAddButton = true, AddCopiesLastElement = false)]
        public List<WaveFormation> WaveFormations = new();

        [Button("Add Wave")]
        private void AddWave()
        {
            var nextLevel = 0;
            if (WaveFormations.Count > 0)
            {
                foreach (var formation in WaveFormations)
                {
                    if (formation.WaveOrder >= nextLevel)
                    {
                        nextLevel = formation.WaveOrder + 1;
                    }
                }
            }

            WaveFormations.Add(new WaveFormation { WaveOrder = nextLevel });
        }

        [Button("Sort by Order")]
        private void SortByOrder()
        {
            WaveFormations.Sort((a, b) => a.WaveOrder.CompareTo(b.WaveOrder));
        }
    }
}