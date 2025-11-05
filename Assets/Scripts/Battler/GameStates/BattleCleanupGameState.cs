using System;
using System.Linq;

namespace AutoBattler
{
    public class BattleCleanupGameState : GameStateBase
    {
        public override event Action<GameStateID> StateChangeRequest;

        private readonly EntitiesService entitiesService;
        private readonly GridService gridService;
           
        public BattleCleanupGameState(ServiceLocator serviceLocator) : base(serviceLocator)
        {
            entitiesService = serviceLocator.EntitiesService;
            gridService = serviceLocator.GridService;
        }

        public override void Enter()
        {
            base.Enter();

            var enemies = entitiesService.SelectUnits(unit => unit.StatusProvider.Faction == UnitFaction.Enemy).ToList();

            foreach (var enemy in enemies)
            {
                var (x, y) = enemy.GridPosition;
                var enemyCell = gridService.GetCellByPosition(x, y);

                enemy.StateMachine.SetState(UnitStateID.None, notify: false);

                entitiesService.RemoveUnit(enemy);
                enemyCell.RemoveEntity();
            }

            var playerUnits = entitiesService.SelectFightingUnits(unit => unit.StatusProvider.Faction == UnitFaction.Player);

            foreach (var unit in playerUnits)
            {
                unit.StateMachine.SetState(UnitStateID.Waiting, notify: false);
            }

            gridService.ResetActiveGridEntities();

            StateChangeRequest?.Invoke(GameStateID.BattlePrepare);

        }
    }
}