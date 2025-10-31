using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitInventoryService : MonoBehaviour
    {
        private ServiceLocator serviceLocator;
        private UnitInventoryView inventoryView;

        private Dictionary<UnitType, int> availableUnits = new Dictionary<UnitType, int>();

        public void Initialize(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void SetView(UnitInventoryView unitInventoryView)
        {
            this.inventoryView = unitInventoryView;
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
            UpdateView();
        }

        public void UpdateView()
        {
            if (inventoryView)
            {
                inventoryView.UpdateUnits(availableUnits);
            }
            else
            {
                Debug.LogError($"{nameof(UnitInventoryService)} : Units in inventory was updated, but no view set, nothing to refresh.");
            }
        }
    }
}