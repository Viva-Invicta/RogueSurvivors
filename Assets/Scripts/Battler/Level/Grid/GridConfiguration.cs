using System;
using UnityEngine;

namespace AutoBattler
{
    [Serializable]
    public class GridConfiguration
    {
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
    }
}