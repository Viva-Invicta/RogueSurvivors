using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DunDungeons
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private EnemySpawnConfig config;

        private EntitiesService entitiesService;
        private UIService uiService;
        private PrefabsService prefabsService;
        private SpawnerService spawnerService;

        private ServiceLocator serviceLocator;

        public void Initialize(ServiceLocator serviceLocator)
        {
            entitiesService = serviceLocator.EntitiesService;
            uiService = serviceLocator.UIService;
            prefabsService = serviceLocator.PrefabsService;
            spawnerService = serviceLocator.SpawnerService;
            this.serviceLocator = serviceLocator;
        }

        public CharacterBehaviourController SpawnSkeleton()
        {
            if (entitiesService.Enemies.Count() >= spawnerService.MaxEnemiesCount)
            {
                return default;
            }

            var spawnPoints = config.SpawnPoints;
            var randomPointIndex = Random.Range(0, spawnPoints.Count);
            var spawnPoint = spawnPoints.ElementAt(randomPointIndex);

            var characterType = config.GetRandomCharacterType();
            var enemyController = GameObject.Instantiate(prefabsService.GetCharacterPrefabByType(characterType));

            var enemyGO = enemyController.gameObject;

            var spawnXPosition = spawnPoint.position.x + Random.Range(-config.SpawnRadius, config.SpawnRadius);
            var spawnZPosition = spawnPoint.position.z + Random.Range(-config.SpawnRadius, config.SpawnRadius);
            var spawnPosition = new Vector3(spawnXPosition, spawnPoint.position.y, spawnZPosition);

            enemyGO.transform.position = spawnPosition;

            entitiesService.AddEnemy(enemyGO);
            uiService.AddHealthBar(enemyGO, enemyGO.GetComponent<HealthComponent>());
            enemyController.Initialize(serviceLocator);

            return enemyController;
        }
        private void OnDrawGizmosSelected()
        {
            foreach (var spawnPoint in config.SpawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, config.SpawnRadius * 2);
            }
        }
    }
}
