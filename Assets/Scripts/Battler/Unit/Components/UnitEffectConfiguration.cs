using UnityEngine;

namespace AutoBattler
{
    [System.Serializable]
    public class UnitEffectConfiguration
    {
        [SerializeField]
        private UnitEffect prefab;

        [SerializeField]
        private UnitEffectType type;

        public UnitEffectType Type => type;
        public UnitEffect Prefab => prefab;
    }
}
