using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitStateFactory : IStateFactory<UnitStateBase, UnitStateID>
    {
        private readonly Dictionary<UnitStateID, Func<UnitStateBase>> factories;

        public UnitStateFactory(UnitStateData stateData)
        {
            factories = new Dictionary<UnitStateID, Func<UnitStateBase>>
            {
                [UnitStateID.Preview] = () => new UnitPreviewState(stateData),
                [UnitStateID.Waiting] = () => new UnitWaitingState(stateData),
                [UnitStateID.Fight] = () => new UnitFightingState(stateData),
                [UnitStateID.Dead] = () => new UnitDeadState(stateData),
                [UnitStateID.None] = () => default
            };
        }

        public UnitStateBase CreateState(UnitStateID state)
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