using DunDungeons;
using System;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class CombatPrepareGameState : GameStateBase
    {
        public override event Action<GameStateID> StateChangeRequest;

        private readonly UIService uiService;
        private readonly UnitPreviewDragService unitPreviewDragService;
        private readonly GridService gridService;
        private readonly EntitiesService entitiesService;
        private readonly EnemyFormationConfigsService enemyFormationConfigsService;
        private readonly UnitPrefabsService unitPrefabsService;
        private readonly RoomsService roomsService;

        private readonly TargetSelectorFactory targetSelectorFactory;

        public CombatPrepareGameState(ServiceLocator serviceLocator) : base(serviceLocator)
        {
            uiService = serviceLocator.UIService;
            unitPreviewDragService = serviceLocator.UnitPreviewDragService;
            gridService = serviceLocator.GridService;
            entitiesService = serviceLocator.EntitiesService;
            enemyFormationConfigsService = serviceLocator.EnemyFormationConfigsService;
            unitPrefabsService = serviceLocator.UnitsPrefabsService;
            roomsService = serviceLocator.RoomsService;

            targetSelectorFactory = new TargetSelectorFactory(entitiesService);
        }

        public override void Enter()
        {
            base.Enter();

            var unitInventory = uiService.CreateOrShowView<UnitInventoryView>(UIViewType.UnitInventory);
            unitInventory.EntryDragged += HandleUnitInventoryEntryDragged;

            var startFightButton = uiService.CreateOrShowView<SimpleButtonView>(UIViewType.StartFightButton);
            startFightButton.Pressed += HandleStartFightButtonPressed;
            startFightButton.Initialize(ServiceLocator);

            unitPreviewDragService.PreviewDragReleased += HandleUnitPreviewReleased;

            SpawnNextEnemiesWave();
        }

        public override void Exit()
        {
            if (uiService.TryGetView<UnitInventoryView>(UIViewType.UnitInventory, out var inventoryView))
            {
                inventoryView.EntryDragged -= HandleUnitInventoryEntryDragged;
                uiService.HideView(UIViewType.UnitInventory);
            }

            if (uiService.TryGetView<SimpleButtonView>(UIViewType.StartFightButton, out var startFightButton))
            {
                startFightButton.Pressed -= HandleStartFightButtonPressed;
                uiService.HideView(UIViewType.StartFightButton);
            }

            unitPreviewDragService.PreviewDragReleased -= HandleUnitPreviewReleased;
           
            base.Exit();
        }

        private void HandleUnitInventoryEntryDragged(UnitType unitType)
        {
            unitPreviewDragService.StartPreviewDrag(unitType);
            gridService.HighlightCells();
        }

        private void HandleUnitPreviewReleased()
        {
            var activePreview = unitPreviewDragService.ActivePreview;

            var layerMask = ~(1 << LayerMask.NameToLayer(PhysicsLayersUtility.UnitPreviewLayer));
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask))
            {
                if (gridService.TryPlaceEntityAtPosition(activePreview.gameObject, hit.point))
                {
                    activePreview.StateMachine.SetState(UnitStateID.Waiting);
                    entitiesService.AddUnit(activePreview);
                }
                else
                {
                    UnityEngine.Object.Destroy(activePreview.gameObject);
                }
            }

            unitPreviewDragService.ClearPreview();
            gridService.DehighlightCells();
        }

        private void SpawnNextEnemiesWave()
        {
            var enemiesFormation = enemyFormationConfigsService.CurrentEnemiesFormation;

            var currentRoomWave = roomsService.CurrentWave;
            var wavesCount = enemiesFormation.WaveFormations.Count;

            int waveToSpawn;

            if (wavesCount > currentRoomWave)
            {
                waveToSpawn = currentRoomWave;
                roomsService.IncreaseWave();
            }
            else
            {
                waveToSpawn = wavesCount - 1;
            }

            var waveFormation = enemiesFormation.WaveFormations.ElementAt(waveToSpawn);
            
            foreach (var enemy in waveFormation.Enemies)
            {
                var enemyInstance = UnityEngine.Object.Instantiate(unitPrefabsService.GetUnitPrefabByType(enemy.UnitType));
                enemyInstance.Initialize(UnitFaction.Enemy, targetSelectorFactory);

                if (gridService.TryPlaceEntityAtGridPosition(enemyInstance.gameObject, enemy.GridX, enemy.GridY))
                {
                    enemyInstance.transform.Rotate(0, 180, 0);
                    entitiesService.AddUnit(enemyInstance);
                }
                else
                {
                    UnityEngine.Object.Destroy(enemyInstance);
                }
            }
        }

        private void HandleStartFightButtonPressed()
        {
            StateChangeRequest?.Invoke(GameStateID.Combat);
        }
    }
}