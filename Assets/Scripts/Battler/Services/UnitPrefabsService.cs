using DunDungeons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace AutoBattler
{
    public class UnitPrefabsService : MonoBehaviour
    {
        [field: SerializeField]
        [AssetsOnly]
        public UnitBehaviourController[] UnitPrefabs
        {
            get;
            private set;
        }

        public UnitBehaviourController GetUnitPrefabByType(UnitType unitType)
        {
            for (var i = 0; i < UnitPrefabs.Length; i++)
            {
                var unit = UnitPrefabs[i];
                if (unit.Configuration.UnitType == unitType)
                {
                    return unit;
                }
            }

            return null;
        }
    }

    public enum UnitType
    {
        Knight0_0
    }
}