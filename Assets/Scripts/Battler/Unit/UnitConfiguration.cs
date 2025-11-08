using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AutoBattler
{
    [CreateAssetMenu(fileName = "UnitConfiguration", menuName = "AutoBattler/UnitConfiguration")]
    public class UnitConfiguration : ScriptableObject
    {
        [SerializeField]
        private UnitType unitType;

        [BoxGroup("Interface Settings")]
        [PreviewField(70)]
        [LabelText("Interface Icon Preview")]
        [SerializeField]
        private Sprite interfaceIcon;

        [BoxGroup("Interface Settings")]
        [LabelText("Interface Description Key")]
        [TextArea(2, 4)]
        [SerializeField]
        private string interfaceDescriptionKey;

        [BoxGroup("In-game settings")]
        [SerializeField]
        private List<UnitDamageConfigurationEntry> baseDamage = new List<UnitDamageConfigurationEntry>();

        [BoxGroup("In-game settings")]
        [SerializeField]
        private float baseAttackCooldown = 1f;

        [BoxGroup("In-game settings")]
        [SerializeField]
        private float baseMovementSpeed = 1f;

        [BoxGroup("In-game settings")]
        [SerializeField]
        private float baseAttackDistance = 1f;

        [BoxGroup("In-game settings")]
        [SerializeField]
        private float baseMaxHealth = 100f;

        [BoxGroup("In-game settings")]
        [SerializeField]
        private float timeToUpdateTarget = 1f;

        [BoxGroup("In-game settings")]
        [SerializeField]
        private TargetSelectorType targetSelector = TargetSelectorType.NearestEnemy;

        [BoxGroup("In-game settings")]
        [SerializeField]
        private List<UnitEffectConfiguration> effects = new List<UnitEffectConfiguration>();

        public UnitType UnitType => unitType;
        public string InterfaceDescriptionKey => interfaceDescriptionKey;
        public Sprite InterfaceIcon => interfaceIcon;
        public List<UnitDamageConfigurationEntry> BaseDamage => baseDamage;
        public float BaseAttackCooldown => baseAttackCooldown;
        public float BaseMovementSpeed => baseMovementSpeed;
        public float BaseAttackDistance => baseAttackDistance;
        public float BaseMaxHealth => baseMaxHealth;
        public float TimeToUpdateTarget => timeToUpdateTarget;
        public TargetSelectorType TargetSelector => targetSelector;
        public List<UnitEffectConfiguration> Effects => effects;

    }

    [Serializable]
    public class UnitDamageConfigurationEntry
    {
        [field: SerializeField] public DamageType DamageType { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}