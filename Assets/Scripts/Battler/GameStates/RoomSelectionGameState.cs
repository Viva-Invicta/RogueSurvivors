using System;

namespace AutoBattler
{
    public class RoomSelectionGameState : GameStateBase
    {
        public override event Action<GameStateID> StateChangeRequest;

        private readonly RoomsService roomsService;
        private readonly GridService gridService;
        private readonly EnemyFormationConfigsService enemyFormationConfigsService;

        public RoomSelectionGameState(ServiceLocator serviceLocator) : base(serviceLocator)
        {
            roomsService = serviceLocator.RoomsService;
            gridService = serviceLocator.GridService;
            enemyFormationConfigsService = serviceLocator.EnemyFormationConfigsService;
        }

        public override void Enter()
        {
            base.Enter();

            roomsService.NextRoomSelected += HandleRoomSelected;
            roomsService.SelectNextRoom();
        }

        public override void Exit()
        {
            base.Exit();

            roomsService.NextRoomSelected -= HandleRoomSelected;
        }

        private void HandleRoomSelected()
        {
            var activeRoomGrid = roomsService.ActiveRoomGrid;
            gridService.SetActiveRoomGrid(activeRoomGrid);
            enemyFormationConfigsService.UpdateFormationConfig(activeRoomGrid.SizeX, activeRoomGrid.SizeY);

            StateChangeRequest?.Invoke(GameStateID.BattlePrepare);
        }
    }
}