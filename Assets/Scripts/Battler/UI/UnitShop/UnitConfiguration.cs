using UnityEngine;

namespace AutoBattler
{
    [CreateAssetMenu(fileName = "UnitConfiguration", menuName = "AutoBattler/UnitConfiguration")]
    public class UnitConfiguration : ScriptableObject
    {
        [field: SerializeField] public UnitType UnitType { get; private set; }
        [field: SerializeField] public Sprite ShopIcon { get; private set; }
    }
}