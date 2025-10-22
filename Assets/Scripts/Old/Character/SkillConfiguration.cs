using UnityEngine;

namespace DunDungeons
{
    public class SkillConfiguration : ScriptableObject
    {
        [field: SerializeField]
        public SkillType SkillType { get; private set; }

        [field: SerializeField]
        public float Duration { get; private set; }

        [field: SerializeField]
        public float InitialCooldown { get; private set; }

        [field: SerializeField]
        public AnimatorKeyIdentifier Animation { get; private set; }
    }
}