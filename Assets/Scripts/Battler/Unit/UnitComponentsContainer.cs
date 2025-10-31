using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    [Serializable]
    public class UnitComponentsContainer
    {
        [field: SerializeField] public UnitAnimationController AnimationController { get; private set; }
        [field: SerializeField] public UnitMovementController MovementController { get; private set; }
        [field: SerializeField] public UnitCombatControllerBase CombatController { get; private set; }
        [field: SerializeField] public UnitDamageReceiver DamageReceiver { get; private set; }

        public void InitializeComponents(IUnitStatusProvider unitStatus)
        {
            var components = new List<MonoBehaviour>
            {
                AnimationController,
                MovementController,
                CombatController,
                DamageReceiver
            };

            foreach (var component in components)
            {
                TryInitializeComponent(component, unitStatus);
            }
        }

        private void TryInitializeComponent(MonoBehaviour component, IUnitStatusProvider unitStatus)
        {
            if (!component)
            {
                Debug.LogError($"{nameof(UnitComponentsContainer)} : One of the components on unit {unitStatus.Configuration.name} is not set");
            }

            if (component is IInitializableWithUnitStatusComponent initializableComponent)
            {
                initializableComponent.Initialize(unitStatus);
            }
        }
    }
}