using System.Collections.Generic;
using System.Linq;

namespace AutoBattler
{
    public class EffectsValueProcessor
    {
        public float ApplyEffects(float baseValue, IEnumerable<UnitStatusEffect> effects)
        {
            var result = baseValue;
            var (multiplicativeEffects, additiveEffects) = GroupEffectsByProcessingType(effects);

            foreach (var effect in multiplicativeEffects)
            {
                result *= effect.Value;
            }

            foreach (var effect in additiveEffects)
            {
                result += effect.Value;
            }

            return result;
        }

        private static (IEnumerable<UnitStatusEffect> multiplicative, IEnumerable<UnitStatusEffect> additive)
            GroupEffectsByProcessingType(IEnumerable<UnitStatusEffect> effects)
        {
            var multiplicativeEffects = effects.Where(e => e.ValueProcessingType == StatusEffectValueProcessingType.Multiply);
            var additiveEffects = effects.Where(e => e.ValueProcessingType == StatusEffectValueProcessingType.Increase);

            return (multiplicativeEffects, additiveEffects);
        }
    }
}