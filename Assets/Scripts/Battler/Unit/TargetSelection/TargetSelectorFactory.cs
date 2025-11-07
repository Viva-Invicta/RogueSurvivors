using System.Collections.Generic;
using UnityEngine;

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
                { TargetSelectorType.RandomEnemy, new RandomEnemySelector(entitiesService) }
            };
        }

        public ITargetSelector GetSelector(TargetSelectorType type)
        {
            if (selectors.TryGetValue(type, out var selector))
            {
                return selector;
            }

            Debug.LogError($"{nameof(TargetSelectorFactory)} : Target selector for type {type} not registered.");
            return default;
        }
    }
}