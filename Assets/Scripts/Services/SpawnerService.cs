using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public class SpawnerService : MonoBehaviour
    {
        [field: SerializeField]
        public int MaxEnemiesCount { get; private set; } = 70;      
    }
}