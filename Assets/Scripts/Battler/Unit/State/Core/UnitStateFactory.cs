using System;
using System.Collections.Generic;

namespace AutoBattler
{
    public class UnitStateFactory : IUnitStateFactory
    {
        private readonly Dictionary<UnitState, Func<UnitBehaviourController, UnitStateBase>> _factories;

        public UnitStateFactory()
        {
            _factories = new Dictionary<UnitState, Func<UnitBehaviourController, UnitStateBase>>
            {
                [UnitState.Preview] = controller => new UnitPreviewState(controller),
                [UnitState.Waiting] = controller => new UnitWaitingState(controller),
                [UnitState.Fight] = controller => new UnitFightingState(controller),
                [UnitState.Dead] = controller => new UnitDeadState(controller)
            };
        }

        public UnitStateBase CreateState(UnitState state, UnitBehaviourController controller)
        {
            if (_factories.TryGetValue(state, out var factory))
                return factory(controller);

            throw new ArgumentException($"No factory registered for state: {state}");
        }
    }
}