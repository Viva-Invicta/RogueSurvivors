using UnityEngine;
using System.Linq;

namespace AutoBattler
{
    public class FarthestAllySelector : ITargetSelector
    {
        private readonly EntitiesService entitiesService;

        public FarthestAllySelector(EntitiesService entitiesService)
        {
            this.entitiesService = entitiesService;
        }

        public UnitBehaviourController SelectTarget(UnitBehaviourController requester)
        {
            var allies = entitiesService
                .SelectUnits(u => u.StatusProvider.Faction == requester.StatusProvider.Faction && u != requester)
                .ToList();

            if (!allies.Any())
            {
                return null;
            }

            return allies
                .OrderByDescending(u => Vector3.Distance(requester.transform.position, u.transform.position))
                .FirstOrDefault();
        }
    }
}