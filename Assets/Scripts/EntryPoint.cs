using UnityEngine;

namespace DunDungeons
{
    public class EntryPoint : MonoBehaviour
    {
        private const string ServiceLocatorTag = "ServiceLocator";
        private const string PlayerTag = "Player";
        private const string CameraTag = "MainCamera";

        private EnemySpawner enemySpawner;

        private void OnEnable()
        {
            var player = GameObject.FindGameObjectWithTag(PlayerTag);
            var camera = GameObject.FindGameObjectWithTag(CameraTag);

            var serviceLocator = GameObject.FindGameObjectWithTag(ServiceLocatorTag).GetComponent<ServiceLocator>();

            serviceLocator.EntitiesService.SetPlayer(player);
            serviceLocator.UIService.AddHealthBar(player, player.GetComponent<HealthComponent>());
            
            camera.GetComponent<CameraController>().Initialize(serviceLocator);

            var playerBehaviour = player.GetComponent<CharacterBehaviourController>();
            if (playerBehaviour)
            {
                playerBehaviour.Initialize(serviceLocator);
            }
            else
            {
                Debug.LogError("No player behaviour component on object with player tag!!!");
            }

            enemySpawner = new EnemySpawner();
            enemySpawner.Initialize(serviceLocator);
            SpawnSkeleton();
        }

        private void SpawnSkeleton()
        {
            var skeletonController = enemySpawner.SpawnSkeleton();
            var skeletonHealth = skeletonController.GetComponent<HealthComponent>();

            skeletonHealth.Updated += () => HandleSkeletonHealthUpdated(skeletonController, skeletonHealth);
        }

        private void HandleSkeletonHealthUpdated(CharacterBehaviourController skeletonController, HealthComponent skeletonHealth)
        {
            if (skeletonHealth.CurrentHP <= 0)
            {
                var skeletonCounter = Random.Range(1, 3);
                for (var skeletonIndex = 0; skeletonIndex < skeletonCounter; skeletonIndex++)
                {
                    SpawnSkeleton();
                }
            }
        }
    }
}