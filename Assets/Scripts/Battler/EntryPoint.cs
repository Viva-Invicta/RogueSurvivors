using DunDungeons;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class EntryPoint : MonoBehaviour
    {
        private ServiceLocator serviceLocator;
        private TargetSelectorFactory targetSelectorFactory;
        private int currentEnemyWave = 0;

        private void OnEnable()
        {
            serviceLocator = CatchServiceLocator();
            serviceLocator.InitializeServices();
        }

        private void Start()
        {
            targetSelectorFactory = new TargetSelectorFactory(serviceLocator.EntitiesService);

            var uiService = serviceLocator.UIService;
            var unitInventoryView = uiService.CreateOrShowView<UnitInventoryView>(UIViewType.UnitInventory);
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

            var startFightButtonView = uiService.CreateOrShowView<SimpleButtonView>(UIViewType.StartFightButton);
            startFightButtonView.Pressed += HandleStartFightButtonPressed;
            startFightButtonView.Initialize(serviceLocator);
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
                    activePreview.StateUpdated += HandleUnitStateChange;
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

            serviceLocator.EnemyFormationConfigsService.SelectFormationConfigForRoom(activeRoomGrid.SizeX, activeRoomGrid.SizeY);

            SpawnNextEnemiesWave();
        }

        private void SpawnNextEnemiesWave()
        {
            var enemiesFormation = serviceLocator.EnemyFormationConfigsService.CurrentEnemiesFormation;

            var wavesInFormationCount = enemiesFormation.WaveFormations.Count();
            var waveIndex = 0;

            if (wavesInFormationCount > currentEnemyWave)
            {
                waveIndex = currentEnemyWave++;
            }
            else
            {
                waveIndex = wavesInFormationCount - 1;
            }

            var waveFormation = enemiesFormation.WaveFormations.ElementAt(waveIndex);
            var prefabsService = serviceLocator.UnitsPrefabsService;
            var gridService = serviceLocator.GridService;

            foreach (var enemy in waveFormation.Enemies)
            {
                var enemyInstance = Instantiate(prefabsService.GetUnitPrefabByType(enemy.UnitType));
                enemyInstance.Initialize(UnitFaction.Enemy, targetSelectorFactory);

                if (gridService.TryPlaceEntityAtGridPosition(enemyInstance.gameObject, enemy.GridX, enemy.GridY))
                {
                    enemyInstance.transform.Rotate(0, 180, 0);
                    enemyInstance.StateUpdated += HandleUnitStateChange;
                    serviceLocator.EntitiesService.AddUnit(enemyInstance);
                }
                else
                {
                    Destroy(enemyInstance);
                }
            }
        }

        private void HandleStartFightButtonPressed()
        {
            foreach (var unit in serviceLocator.EntitiesService.Units)
            {
                unit.SetState(UnitState.Fight, notificate: false);
            }

            var uiService = serviceLocator.UIService;

            uiService.HideView(UIViewType.StartFightButton);
            uiService.HideView(UIViewType.UnitInventory);
        }

        private void HandleUnitStateChange()
        {
            var playerFightingUnitsCount = 0;
            var enemyFightingUnitsCount = 0;

            var entitiesService = serviceLocator.EntitiesService;
            playerFightingUnitsCount = entitiesService.SelectFightingUnits(unit => unit.StatusProvider.Faction == UnitFaction.Player).Count();
            enemyFightingUnitsCount = entitiesService.SelectFightingUnits(unit => unit.StatusProvider.Faction == UnitFaction.Enemy).Count();

            if (playerFightingUnitsCount == 0 || enemyFightingUnitsCount == 0)
            {
                var uiService = serviceLocator.UIService;
                var gridService = serviceLocator.GridService;

                var enemies = entitiesService.SelectUnits(unit => unit.StatusProvider.Faction == UnitFaction.Enemy).ToList();

                foreach (var enemy in enemies)
                {
                    var (x, y) = enemy.GridPosition;
                    var enemyCell = gridService.GetCellByPosition(x, y);

                    enemy.SetState(UnitState.None, notificate: false);

                    entitiesService.RemoveUnit(enemy);
                    enemyCell.RemoveEntity();
                }

                gridService.ResetActiveGridEntities();
                var playerUnits = entitiesService.SelectFightingUnits(unit => unit.StatusProvider.Faction == UnitFaction.Player);
                  
                foreach (var unit in playerUnits)
                {
                    unit.SetState(UnitState.Waiting, notificate: false);
                }

                SpawnNextEnemiesWave();
                uiService.CreateOrShowView<UnitInventoryView>(UIViewType.UnitInventory);
                uiService.CreateOrShowView<SimpleButtonView>(UIViewType.StartFightButton);
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