using UnityEngine;
using System.Linq;

namespace AutoBattler
{
    public class ClosestEnemySelector : ITargetSelector
    {
        private readonly EntitiesService entitiesService;

        public ClosestEnemySelector(EntitiesService entitiesService)
        {
            this.entitiesService = entitiesService;
        }

        public UnitBehaviourController SelectTarget(UnitBehaviourController requester)
        {
            var enemies = entitiesService
                .SelectUnits(unit => unit.StatusProvider.Faction != requester.StatusProvider.Faction)
                .ToList();

            if (!enemies.Any())
            {
                return null;
            }

            return enemies
                .OrderBy(unit => Vector3.Distance(requester.transform.position, unit.transform.position))
                .FirstOrDefault();
        }
    }
}