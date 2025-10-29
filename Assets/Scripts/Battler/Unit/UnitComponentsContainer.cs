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
        [field: SerializeField] public UnitCombatController CombatController { get; private set; }

        public void InitializeComponents(IUnitStatusProvider unitStatus)
        {
            var components = new List<MonoBehaviour>
            {
                AnimationController,
                MovementController,
                CombatController
            };

            foreach (var component in components)
            {
                TryInitializeComponent(component, unitStatus);
            }
        }

        private void TryInitializeComponent(MonoBehaviour component, IUnitStatusProvider unitStatus)
        {
            if (component is IInitializableWithUnitStatusComponent initializableComponent)
            {
                initializableComponent.Initialize(unitStatus);
            }
        }
    }
}