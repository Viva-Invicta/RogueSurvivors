using UnityEngine;

namespace AutoBattler
{
    public class EntryPoint : MonoBehaviour
    {
        private ServiceLocator serviceLocator;

        private void OnEnable()
        {
            serviceLocator = CatchServiceLocator();
            serviceLocator.InitializeServices();
        }

        private void Start()
        {
            var unitShopView = serviceLocator.UIService.CreateOrShowView<UnitShopView>(UIViewType.UnitShop);
            unitShopView.Initialize(serviceLocator);

            var unitsShopService = serviceLocator.UnitShopService;

            unitsShopService.SetView(unitShopView);
            unitsShopService.AddAvailableUnit(UnitType.Knight0_0);

            unitShopView.EntryDragged += HandleUnitShopEntryDragged;
            serviceLocator.UnitPreviewDragService.PreviewDragReleased += HandleUnitPreviewReleased;

            var roomsService = serviceLocator.RoomsService;
            roomsService.NextRoomSelected += HandleRoomSelected;
            roomsService.SelectNextRoom();
        }

        private void HandleUnitShopEntryDragged(UnitType unitType)
        {
            serviceLocator.UnitPreviewDragService.StartPreviewDrag(unitType);
            serviceLocator.GridService.HighlightCells();
        }

        private void HandleUnitPreviewReleased()
        {
            var gridService = serviceLocator.GridService;
            var unitPreviewDragService = serviceLocator.UnitPreviewDragService;
            var activePreview = unitPreviewDragService.ActivePreview;

            var layerMask = ~(1 << LayerMask.NameToLayer(PhysicsLayersUtility.UnitPreviewLayer));
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
            {
                if (gridService.TryPlaceEntityAtPosition(activePreview.gameObject, hit.point))
                {
                    activePreview.IsPreview = false;
                }
                else
                {
                    Destroy(activePreview.gameObject);
                }
            }

            unitPreviewDragService.ClearPreview();
            serviceLocator.GridService.DehighlightCells();
        }

        private void HandleRoomSelected()
        {
            serviceLocator.GridService.SetActiveRoomGrid(serviceLocator.RoomsService.ActiveRoomGrid);
        }

        private ServiceLocator CatchServiceLocator()
        {
            var serviceLocatorTag = TagsUtility.ServiceLocatorTag;

            var serviceLocatorGO = GameObject.FindGameObjectWithTag(serviceLocatorTag);
            if (!serviceLocatorGO)
            {
                Debug.LogError($"{nameof(EntryPoint)} : Could not find game object with tag {serviceLocatorTag}");
                return default;
            }

            var serviceLocator = serviceLocatorGO.GetComponent<ServiceLocator>();
            if (!serviceLocator)
            {
                Debug.LogError($"{nameof(EntryPoint)} : Object with tag {serviceLocatorTag} does not contain {nameof(ServiceLocator)} component");
                return default;
            }

            return serviceLocator;
        }
    }
}