using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private ServiceLocator serviceLocator;

        private StateMachine<GameStateBase, GameStateID> gameStateMachine;

        private void OnEnable()
        {
            serviceLocator.InitializeServices();
        }

        private void Start()
        {
            var gameStateFactory = new GameStateFactory(serviceLocator);
            gameStateMachine = new StateMachine<GameStateBase, GameStateID>(gameStateFactory);

            gameStateMachine.StateChanged += HandleGameStateChange;
            gameStateMachine.SetState(GameStateID.Initialization);
        }

        private void Update()
        {
            gameStateMachine?.Process(Time.deltaTime);
        }

        private void HandleGameStateChange(GameStateID newStateID)
        {
            Debug.Log($"{nameof(EntryPoint)}:GameState changed, new state is {newStateID}");
        }
    }
}