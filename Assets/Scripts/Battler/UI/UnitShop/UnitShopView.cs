using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace AutoBattler
{
    public class UnitShopView : UIView
    {
        public event Action<UnitType> EntryDragged;

        [SerializeField]
        private UIViewType viewType = UIViewType.UnitShop;

        public override UIViewType ViewType => viewType;

        private UnitPrefabsService unitsPrefabsService;
        private UIPrefabsService uiPrefabsService;

        private HashSet<UnitShopEntryView> unitShopEntryViews = new HashSet<UnitShopEntryView>();

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
                var views = unitShopEntryViews.Where(entryView => entryView.UnitType == unitType);

                var unitsCount = unitTypeCountPair.Value;
                if (views.Count() > unitsCount)
                {
                    var lastView = unitShopEntryViews.Last();
                    unitShopEntryViews.Remove(lastView);
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
            var uiEntryPrefab = uiPrefabsService.GetUIPrefabByType(UIViewType.UnitShopEntry);

            var shopEntry = Instantiate(uiEntryPrefab).GetComponent<UnitShopEntryView>();
            shopEntry.transform.SetParent(transform, false);
            shopEntry.DragStarted += () => HandleEntryDragged(shopEntry.UnitType);

            shopEntry.Initialize(viewType, unitConfiguration.ShopIcon);
        }
        
        private void HandleEntryDragged(UnitType unitType)
        {
            EntryDragged?.Invoke(unitType);
        }
    }
}