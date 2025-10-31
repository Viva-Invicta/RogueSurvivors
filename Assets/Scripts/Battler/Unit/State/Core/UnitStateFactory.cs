using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitStateFactory : IUnitStateFactory
    {
        private readonly Dictionary<UnitState, Func<UnitStateBase>> factories;

        public UnitStateFactory(UnitStateData stateData)
        {
            factories = new Dictionary<UnitState, Func<UnitStateBase>>
            {
                [UnitState.Preview] = () => new UnitPreviewState(stateData),
                [UnitState.Waiting] = () => new UnitWaitingState(stateData),
                [UnitState.Fight] = () => new UnitFightingState(stateData),
                [UnitState.Dead] = () => new UnitDeadState(stateData),
                [UnitState.None] = () => default
            };
        }

        public UnitStateBase CreateState(UnitState state)
        {
            if (factories.TryGetValue(state, out var factory))
            {
                return factory();
            }

            Debug.LogError($"{nameof(UnitStateFactory)} : Could not create state {state} because there's no factory for it");

            return default;
        }
    }
}