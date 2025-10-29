using System.Collections.Generic;
using System;

namespace AutoBattler
{
    public class TargetSelectorFactory
    {
        private readonly Dictionary<TargetSelectorType, ITargetSelector> selectors;

        public TargetSelectorFactory(EntitiesService entitiesService)
        {
            selectors = new Dictionary<TargetSelectorType, ITargetSelector>
            {
                { TargetSelectorType.NearestEnemy, new ClosestEnemySelector(entitiesService) },
                { TargetSelectorType.FarthestAlly, new FarthestAllySelector(entitiesService) },
            };
        }

        public ITargetSelector GetSelector(TargetSelectorType type)
        {
            if (selectors.TryGetValue(type, out var selector))
            {
                return selector;
            }

            throw new InvalidOperationException($"{nameof(TargetSelectorFactory)} : Target selector for type {type} not registered.");
        }
    }
}