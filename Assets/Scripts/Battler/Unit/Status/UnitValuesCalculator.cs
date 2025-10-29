using System.Collections.Generic;
using System.Linq;

namespace AutoBattler
{
    public class UnitValuesCalculator
    {
        public List<UnitStatusEffect> Effects { get; private set; } = new List<UnitStatusEffect>();

        private readonly IUnitStatusProvider unitStatusProvider;
        private readonly EffectsValueProcessor effectsValueProcessor;

        public UnitValuesCalculator(IUnitStatusProvider unitStatusProvider)
        {
            this.unitStatusProvider = unitStatusProvider;
            effectsValueProcessor = new EffectsValueProcessor();
        }

        public float CalculateAttackCooldown()
        {
            var relevantEffects = Effects
                .Where(effect => effect.EffectType == UnitStatusEffectType.AttackCooldown);

            return effectsValueProcessor.ApplyEffects(
                unitStatusProvider.BaseAttackCooldown,
                relevantEffects
            );
        }

        public float CalculateIncomingDamage(float baseDamage, DamageType damageType)
        {
            return CalculateDamage(baseDamage, damageType, DamageStatusEffectDirection.Incoming);
        }

        public float CalculateOutcomingDamage(DamageType damageType)
        {
            var baseDamage = unitStatusProvider.BaseDamage.TryGetValue(damageType, out var value) ? value : 1;
            return CalculateDamage(baseDamage, damageType, DamageStatusEffectDirection.Outcoming);
        }

        private float CalculateDamage(float baseDamage, DamageType damageType, DamageStatusEffectDirection direction)
        {
            var relevantEffects = GetRelevantDamageEffects(direction, damageType);
            return effectsValueProcessor.ApplyEffects(baseDamage, relevantEffects);
        }

        private IEnumerable<DamageStatusEffect> GetRelevantDamageEffects(
            DamageStatusEffectDirection direction,
            DamageType damageType)
        {
            return Effects
                .OfType<DamageStatusEffect>()
                .Where(effect => effect.DamageDirection == direction &&
                                (damageType == DamageType.All || effect.DamageType == damageType));
        }

        public void AddEffect(UnitStatusEffect effect)
        {
            Effects.Add(effect);
        }
    }

    public class UnitStatusEffect
    {
        public UnitStatusEffectType EffectType { get; protected set; }
        public StatusEffectValueProcessingType ValueProcessingType { get; protected set; }
        public float Value { get; protected set; }
    }

    public class DamageStatusEffect : UnitStatusEffect
    {
        public DamageType DamageType { get; protected set; }
        public DamageStatusEffectDirection DamageDirection { get; protected set; }
    }

    public enum UnitStatusEffectType
    {
        Damage,
        AttackCooldown,
        SkillRadius
    }

    public enum DamageStatusEffectDirection
    {
        Incoming,
        Outcoming
    }

    public enum StatusEffectValueProcessingType
    {
        Increase,
        Multiply
    }

    public enum DamageType
    {
        All,
        Physical,
        Fire,
        Ice,
        Electricity,
        Curse,
        Arcane
    }
}