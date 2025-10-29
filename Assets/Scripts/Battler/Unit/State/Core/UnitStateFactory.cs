using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitStateFactory : IUnitStateFactory
    {
        private readonly Dictionary<UnitState, Func<UnitBehaviourController, UnitStateBase>> factories;

        public UnitStateFactory()
        {
            factories = new Dictionary<UnitState, Func<UnitBehaviourController, UnitStateBase>>
            {
                [UnitState.Preview] = controller => new UnitPreviewState(controller),
                [UnitState.Waiting] = controller => new UnitWaitingState(controller),
                [UnitState.Fight] = controller => new UnitFightingState(controller),
                [UnitState.Dead] = controller => new UnitDeadState(controller)
            };
        }

        public UnitStateBase CreateState(UnitState state, UnitBehaviourController controller)
        {
            if (factories.TryGetValue(state, out var factory))
            {
                return factory(controller);
            }

            Debug.LogError($"{nameof(UnitStateFactory)} : Could not create state {state} because there's no factory for it");

            return default;
        }
    }
}