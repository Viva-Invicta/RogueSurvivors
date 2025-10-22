using System;
using System.Collections.Generic;

namespace DunDungeons
{
    public static class SkillsUtility
    {
        private static Dictionary<SkillType, Func<SkillConfiguration, ISkill>> SkillFactories = new()
        {
            { SkillType.Juggernaut, (SkillConfiguration config) => new JuggernautSkill(config) }
        };

        public static bool TryGetSkillByConfiguration(SkillConfiguration configuration, out ISkill skill)
        {
            if (SkillFactories.TryGetValue(configuration.SkillType, out var SkillFactory))
            {
                skill = SkillFactory(configuration);
                return true;
            }

            skill = default;
            return false;
        }
    }

    public enum SkillType
    {
        Juggernaut
    }
}