using System;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class GameStateFactory : IStateFactory<GameStateBase, GameStateID>
    {
        private Dictionary<GameStateID, Func<GameStateBase>> factoryMethods;

        public GameStateFactory(ServiceLocator serviceLocator)
        {
            factoryMethods = new Dictionary<GameStateID, Func<GameStateBase>>
            { 
                [GameStateID.Initialization] = () => new InitializationGameState(serviceLocator),
                [GameStateID.RoomSelection] = () => new RoomSelectionGameState(serviceLocator),
                [GameStateID.BattlePrepare] = () => new BattlePrepareGameState(serviceLocator),
                [GameStateID.Battle] = () => new BattleGameState(serviceLocator),
                [GameStateID.BattleCleanup] = () => new BattleCleanupGameState(serviceLocator)
            };
        }

        public  GameStateBase CreateState(GameStateID stateID)
        {
            if (factoryMethods.TryGetValue(stateID, out var factoryMethod))
            {
                return factoryMethod();
            }
            else
            {
                Debug.LogError($"{nameof(GameStateFactory)}: No factory method for game state with id {stateID}");
                return default;
            }
        }
    }
}