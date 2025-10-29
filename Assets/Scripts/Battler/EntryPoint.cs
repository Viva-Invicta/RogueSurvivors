using System.Linq;
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
            var unitInventoryView = serviceLocator.UIService.CreateOrShowView<UnitInventoryView>(UIViewType.UnitInventory);
            unitInventoryView.Initialize(serviceLocator);

            var unitInventoryService = serviceLocator.UnitInventoryService;

            unitInventoryService.SetView(unitInventoryView);
            unitInventoryService.AddAvailableUnit(UnitType.Skeleton0_0);
            unitInventoryService.AddAvailableUnit(UnitType.Knight0_0);

            unitInventoryView.EntryDragged += HandleUnitInventoryEntryDragged;
            serviceLocator.UnitPreviewDragService.PreviewDragReleased += HandleUnitPreviewReleased;

            var roomsService = serviceLocator.RoomsService;
            roomsService.NextRoomSelected += HandleRoomSelected;
            roomsService.SelectNextRoom();
        }

        private void HandleUnitInventoryEntryDragged(UnitType unitType)
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
                    activePreview.SetState(UnitState.Waiting);
                    serviceLocator.EntitiesService.AddUnit(activePreview);
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
            var activeRoomGrid = serviceLocator.RoomsService.ActiveRoomGrid;
            var gridService = serviceLocator.GridService;
            var prefabsService = serviceLocator.UnitsPrefabsService;

            gridService.SetActiveRoomGrid(activeRoomGrid);

            var enemyFormationConfiguration = serviceLocator.EnemyFormationConfigsService.GetFormationConfigForRoom(activeRoomGrid.SizeX, activeRoomGrid.SizeY);

            var enemyFormation = enemyFormationConfiguration.WaveFormations.First();
            foreach (var enemy in enemyFormation.Enemies)
            {
                var enemyInstance = Instantiate(prefabsService.GetUnitPrefabByType(enemy.UnitType));
                enemyInstance.Initialize(UnitFaction.Enemy);
                gridService.TryPlaceEntityAtGridPosition(enemyInstance.gameObject, enemy.GridX, enemy.GridY);
                enemyInstance.transform.Rotate(0, 180, 0);
                serviceLocator.EntitiesService.AddUnit(enemyInstance);
            }
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