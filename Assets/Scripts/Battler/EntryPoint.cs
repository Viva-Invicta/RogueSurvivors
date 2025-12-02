using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField]
        private ServiceLocator serviceLocator;

        [SerializeField]
        private Transform cameraPrepareTransform;

        [SerializeField]
        private Transform cameraFightTransform;

        private Transform lookAt;

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

            if (lookAt)
            {
                Camera.main.transform.DOLookAt(lookAt.position, 0);
            }
        }

        private void HandleGameStateChange(GameStateID newStateID)
        {
            if (newStateID == GameStateID.Combat)
            {
                var units = serviceLocator.EntitiesService.Units.Where(unit => unit.Configuration.UnitType == UnitType.Amir);

                if (units.Any())
                {
                    lookAt = units.First().transform;
                }
                Camera.main.transform.DOMove(cameraFightTransform.position, 0.5f);
                Camera.main.transform
                    .DORotate(cameraFightTransform.rotation.eulerAngles, 0.5f);
            }
            else
            {
                lookAt = null;
                Camera.main.transform.DOMove(cameraPrepareTransform.position, 0.5f);
                Camera.main.transform
                    .DORotate(cameraPrepareTransform.rotation.eulerAngles, 0.5f);
            }

            Debug.Log($"{nameof(EntryPoint)}:GameState changed, new state is {newStateID}");
        }
    }
}