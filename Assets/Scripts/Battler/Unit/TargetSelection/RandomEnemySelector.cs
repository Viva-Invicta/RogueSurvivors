using UnityEngine;
using System.Linq;

namespace AutoBattler
{
    public class RandomEnemySelector : ITargetSelector
    {
        private readonly EntitiesService entitiesService;

        public RandomEnemySelector(EntitiesService entitiesService)
        {
            this.entitiesService = entitiesService;
        }

        public UnitBehaviourController SelectTarget(UnitBehaviourController requester)
        {
            var enemies = entitiesService
                .SelectFightingUnits(unit => unit.StatusProvider.Faction != requester.StatusProvider.Faction)
                .ToList();

            if (!enemies.Any())
            {
                return null;
            }

            // Выбираем случайного врага
            int randomIndex = Random.Range(0, enemies.Count);
            return enemies[randomIndex];
        }
    }
}