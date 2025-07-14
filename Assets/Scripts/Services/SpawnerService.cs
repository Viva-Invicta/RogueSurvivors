using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public class SpawnerService : MonoBehaviour
    {
        [field: SerializeField]
        [SceneObjectsOnly]
        public List<Transform> SpawnPoints { get; private set; }

        [field: SerializeField]
        public float SpawnRadius { get; private set; } = 1f;

        private void OnDrawGizmosSelected()
        {
            foreach (var spawnPoint in SpawnPoints)
            {
                Gizmos.DrawWireSphere(spawnPoint.position, SpawnRadius * 2);
            }
        }
    }
}