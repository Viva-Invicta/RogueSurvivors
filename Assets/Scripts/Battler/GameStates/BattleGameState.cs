using DunDungeons;
using System;
using System.Linq;

namespace AutoBattler
{
    public class BattleGameState : GameStateBase
    {
        public override event Action<GameStateID> StateChangeRequest;

        private readonly EntitiesService entitiesService;

        public BattleGameState(ServiceLocator serviceLocator) : base(serviceLocator)
        {
            entitiesService = serviceLocator.EntitiesService;

        }

        public override void Enter()
        {
            base.Enter();

            foreach (var unit in entitiesService.Units)
            {
                unit.StateMachine.SetState(UnitStateID.Fight, notify: false);
                unit.StateMachine.StateChanged += HandleUnitStateChange;
            }
        }

        public override void Exit()
        {
            foreach (var unit in entitiesService.Units)
            {
                unit.StateMachine.StateChanged -= HandleUnitStateChange;
            }

            base.Exit();
        }

        private void HandleUnitStateChange(UnitStateID newState)
        {
            if (newState != UnitStateID.Dead)
            {
                return;
            }

            var playerFightingUnitsCount = 0;
            var enemyFightingUnitsCount = 0;

            playerFightingUnitsCount = entitiesService.SelectFightingUnits(unit => unit.StatusProvider.Faction == UnitFaction.Player).Count();
            enemyFightingUnitsCount = entitiesService.SelectFightingUnits(unit => unit.StatusProvider.Faction == UnitFaction.Enemy).Count();

            if (playerFightingUnitsCount == 0 || enemyFightingUnitsCount == 0)
            {
                StateChangeRequest?.Invoke(GameStateID.BattleCleanup);
            }
        }
    }
}