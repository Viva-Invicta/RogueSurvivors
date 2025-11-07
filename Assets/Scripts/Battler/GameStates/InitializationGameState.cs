using System;

namespace AutoBattler
{
    public class InitializationGameState : GameStateBase
    {
        public override event Action<GameStateID> StateChangeRequest;

        private readonly UIService uiService;
        private readonly UnitInventoryService unitInventoryService;

        public InitializationGameState(ServiceLocator serviceLocator) : base(serviceLocator)
        {
            uiService = serviceLocator.UIService;
            unitInventoryService = serviceLocator.UnitInventoryService;
        }

        public override void Enter()
        {
            base.Enter();

            var unitInventoryView = uiService.CreateOrShowView<UnitInventoryView>(UIViewType.UnitInventory);
            unitInventoryView.Initialize(ServiceLocator);

            unitInventoryService.SetView(unitInventoryView);
            unitInventoryService.AddAvailableUnit(UnitType.Skeleton0_0);
            unitInventoryService.AddAvailableUnit(UnitType.Knight0_0);
            unitInventoryService.AddAvailableUnit(UnitType.Mage0_0);

            StateChangeRequest?.Invoke(GameStateID.RoomSelection);
        }
    }
}