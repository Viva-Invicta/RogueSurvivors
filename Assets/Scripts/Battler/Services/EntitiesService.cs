using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class EntitiesService : MonoBehaviour
    {
        private List<UnitBehaviourController> units = new List<UnitBehaviourController>();

        public IReadOnlyList<UnitBehaviourController> Units => units; 

        public void AddUnit(UnitBehaviourController unit)
        {
            if (!units.Contains(unit))
            {
                units.Add(unit);
            }
        }

        public void RemoveUnit(UnitBehaviourController unit)
        {
            if (units.Contains(unit))
            {
                units.Remove(unit);
            }
        }

        public IEnumerable<UnitBehaviourController> SelectUnits(Func<UnitBehaviourController, bool> predicate)
        {
            return units.Where(predicate);
        }
    }
}