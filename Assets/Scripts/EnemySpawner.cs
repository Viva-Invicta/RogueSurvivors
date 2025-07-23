using System.Linq;
using UnityEngine;

namespace DunDungeons
{
    public class EnemySpawner
    {
        private SpawnerService spawnerService;
        private EntitiesService entitiesService;
        private UIService uiService;
        private PrefabsService prefabsService;

        private ServiceLocator serviceLocator;

        public void Initialize(ServiceLocator serviceLocator)
        {
            spawnerService = serviceLocator.SpawnerService;
            entitiesService = serviceLocator.EntitiesService;
            uiService = serviceLocator.UIService;
            prefabsService = serviceLocator.PrefabsService;
            this.serviceLocator = serviceLocator;
        }

        public CharacterBehaviourController SpawnSkeleton()
        {
            var spawnPoints = spawnerService.SpawnPoints;

            var randomPointIndex = Random.Range(0, spawnPoints.Count);
            var spawnPoint = spawnPoints.ElementAt(randomPointIndex);

            var character = (Random.Range(0, 10) > 0 ? CharacterType.Skeleton : CharacterType.BigSkeleton);
            var enemyController = GameObject.Instantiate(prefabsService.GetCharacterPrefabByType(character));

            var enemyGO = enemyController.gameObject;

            var spawnXPosition = spawnPoint.position.x + Random.Range(-spawnerService.SpawnRadius, spawnerService.SpawnRadius);
            var spawnZPosition = spawnPoint.position.z + Random.Range(-spawnerService.SpawnRadius, spawnerService.SpawnRadius);
            var spawnPosition = new Vector3(spawnXPosition, spawnPoint.position.y, spawnZPosition);
       

            enemyGO.transform.position = spawnPosition;

            entitiesService.AddEnemy(enemyGO);
            uiService.AddHealthBar(enemyGO, enemyGO.GetComponent<HealthComponent>());
            enemyController.Initialize(serviceLocator);

            return enemyController;
        }
    }
}