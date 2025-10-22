using System;
using UnityEngine;

namespace AutoBattler
{
    [Serializable]
    public class LevelData 
    {
        [field: SerializeField] public GridConfiguration GridData { get; private set; }
    }
}