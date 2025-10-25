using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace AutoBattler
{
    public class UnitInventoryView : UIView
    {
        public event Action<UnitType> EntryDragged;

        [SerializeField]
        private UIViewType viewType = UIViewType.UnitInventory;

        public override UIViewType ViewType => viewType;

        private UnitPrefabsService unitsPrefabsService;
        private UIPrefabsService uiPrefabsService;

        private HashSet<UnitInventoryEntryView> entries = new HashSet<UnitInventoryEntryView>();

        public override void Initialize(ServiceLocator serviceLocator)
        {
            unitsPrefabsService = serviceLocator.UnitsPrefabsService;
            uiPrefabsService = serviceLocator.UIPrefabsService;
        }

        public override void Release()
        {
            EntryDragged = null;
        }

        public void UpdateUnits(Dictionary<UnitType, int> actualUnits)
        {
            foreach (var unitTypeCountPair in actualUnits)
            {
                var unitType = unitTypeCountPair.Key;
                var views = entries.Where(entryView => entryView.UnitType == unitType);

                var unitsCount = unitTypeCountPair.Value;
                if (views.Count() > unitsCount)
                {
                    var lastView = entries.Last(entryView => entryView.UnitType == unitType);
                    entries.Remove(lastView);
                    lastView.Release();

                    Destroy(lastView.gameObject);
                }
                else if (views.Count() < unitsCount)
                {
                    var countToAdd = unitsCount - views.Count();
                    while (countToAdd-- > 0)
                    {
                        AddView(unitType);
                    }
                }
            }
        }

        private void AddView(UnitType viewType)
        {
            var unitConfiguration = unitsPrefabsService.GetUnitPrefabByType(viewType).Configuration;
            var uiEntryPrefab = uiPrefabsService.GetUIPrefabByType(UIViewType.UnitInventoryEntry);

            var inventoryEntry = Instantiate(uiEntryPrefab).GetComponent<UnitInventoryEntryView>();
            entries.Add(inventoryEntry);
            inventoryEntry.transform.SetParent(transform, false);
            inventoryEntry.DragStarted += () => HandleEntryDragged(inventoryEntry.UnitType);

            inventoryEntry.Initialize(viewType, unitConfiguration.InterfaceIcon);
        }
        
        private void HandleEntryDragged(UnitType unitType)
        {
            EntryDragged?.Invoke(unitType);
        }
    }
}