using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitShopService : MonoBehaviour
    {
        private ServiceLocator serviceLocator;
        private UnitShopView unitShopView;

        private Dictionary<UnitType, int> availableUnits = new Dictionary<UnitType, int>();

        public void Initialize(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void SetView(UnitShopView unitShopView)
        {
            this.unitShopView = unitShopView;
            UpdateView();
        }

        public void AddAvailableUnit(UnitType unitType)
        {
            if (availableUnits.ContainsKey(unitType))
            {
                availableUnits[unitType]++;
            }
            else
            {
                availableUnits.Add(unitType, 1);
            }
            UpdateView();
        }

        public void RemoveAvailableUnit(UnitType unitType)
        {
            if (availableUnits.TryGetValue(unitType, out var unitsCount))
            {
                if (unitsCount > 1)
                {
                    availableUnits[unitType]--;
                }
                else
                {
                    availableUnits.Remove(unitType);
                }
            }
        }

        public void UpdateView()
        {
            unitShopView.UpdateUnits(availableUnits);
        }
    }
}