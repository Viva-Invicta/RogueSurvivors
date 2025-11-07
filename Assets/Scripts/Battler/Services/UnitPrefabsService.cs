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

                if (!unit.Configuration)
                {
                    Debug.LogError($"{nameof(UnitPrefabsService)} :" +
                        $" Unit prefab with name {unit.gameObject.name} does not have configuration!");
                }
                if (unit.Configuration.UnitType == unitType)
                {
                    return unit;
                }
            }

            Debug.LogError($"{nameof(UnitPrefabsService)} : Couldn't find unit {unitType} in registry");
            return default;
        }
    }

    public enum UnitType
    {
        Knight0_0,
        Skeleton0_0,
        Mage0_0
    }
}