using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace AutoBattler
{
    [CreateAssetMenu(menuName = "AutoBattler/Enemy Formation Config", fileName = "EnemyFormation")]
    public class EnemyFormationConfig : ScriptableObject
    {
        [TableList(AlwaysExpanded = true)]
        public List<EnemyUnitFormationEntry> Enemies = new();
    }
}