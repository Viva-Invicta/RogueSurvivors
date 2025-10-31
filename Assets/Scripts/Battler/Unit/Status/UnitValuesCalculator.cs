using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class UnitValuesCalculator
    {
        private readonly UnitConfiguration configuration;
        private readonly Dictionary<DamageType, float> baseDamageDictionary;
        private readonly IUnitStatusProvider unitStatusProvider;
        private readonly EffectsValueProcessor effectsValueProcessor;

        private readonly float baseMovementSpeed;

        public List<UnitStatusEffect> Effects { get; private set; } = new List<UnitStatusEffect>();

        public UnitValuesCalculator(IUnitStatusProvider unitStatusProvider, UnitConfiguration configuration)
        {
            if (unitStatusProvider == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : UnitStatusProvider is null");
                return;
            }

            if (configuration == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : Configuration is null");
                return;
            }

            this.unitStatusProvider = unitStatusProvider;
            this.configuration = unitStatusProvider.Configuration ?? configuration;
            effectsValueProcessor = new EffectsValueProcessor();

            baseDamageDictionary = new Dictionary<DamageType, float>();
            if (configuration.BaseDamage == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : BaseDamage collection is null");
                return;
            }

            foreach (var baseDamageEntry in configuration.BaseDamage)
            {
                if (baseDamageEntry == default)
                {
                    Debug.LogError($"{nameof(UnitValuesCalculator)} : BaseDamage entry is null");
                    continue;
                }

                baseDamageDictionary[baseDamageEntry.DamageType] = baseDamageEntry.Value;
            }

            baseMovementSpeed = configuration.BaseMovementSpeed;
        }

        public float CalculateMovementSpeed()
        {
            return baseMovementSpeed;
        }

        public float CalculateAttackCooldown()
        {
            if (configuration == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : Configuration is null");
                return default;
            }

            if (effectsValueProcessor == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : EffectsValueProcessor is null");
                return default;
            }

            var relevantEffects = Effects
                .Where(effect => effect != null && effect.EffectType == UnitStatusEffectType.AttackCooldown);

            return effectsValueProcessor.ApplyEffects(
                configuration.BaseAttackCooldown,
                relevantEffects
            );
        }

        public float CalculateIncomingDamage(float baseDamage, DamageType damageType)
        {
            if (configuration == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : Configuration is null");
                return default;
            }

            return CalculateDamage(baseDamage, damageType, DamageStatusEffectDirection.Incoming);
        }

        public float CalculateOutcomingDamage(DamageType damageType)
        {
            if (baseDamageDictionary == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : BaseDamageDictionary is null");
                return default;
            }

            var baseDamage = baseDamageDictionary.TryGetValue(damageType, out var value) ? value : 1f;
            return CalculateDamage(baseDamage, damageType, DamageStatusEffectDirection.Outcoming);
        }

        private float CalculateDamage(float baseDamage, DamageType damageType, DamageStatusEffectDirection direction)
        {
            if (effectsValueProcessor == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : EffectsValueProcessor is null");
                return default;
            }

            var relevantEffects = GetRelevantDamageEffects(direction, damageType);
            return effectsValueProcessor.ApplyEffects(baseDamage, relevantEffects);
        }

        private IEnumerable<DamageStatusEffect> GetRelevantDamageEffects(
            DamageStatusEffectDirection direction,
            DamageType damageType)
        {
            return Effects
                .OfType<DamageStatusEffect>()
                .Where(effect => effect != null &&
                               effect.DamageDirection == direction &&
                               (damageType == DamageType.All || effect.DamageType == damageType));
        }

        public void AddEffect(UnitStatusEffect effect)
        {
            if (effect == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : Effect is null");
                return;
            }

            Effects.Add(effect);
        }

        public void RemoveEffect(UnitStatusEffect effect)
        {
            if (effect == default)
            {
                Debug.LogError($"{nameof(UnitValuesCalculator)} : Effect is null");
                return;
            }

            Effects.Remove(effect);
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