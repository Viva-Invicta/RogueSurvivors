using UnityEngine;

public class ValeraSpawner : MonoBehaviour
{
    [SerializeField]
    private ValeraCollectable collectable;

    [SerializeField]
    private float spawnRadius;

    private void OnEnable()
    {
        SpawnCollectable();
    }

    private void SpawnCollectable()
    {
        var collectableInstance = Instantiate(collectable);

        var randomX = Random.Range(-spawnRadius, spawnRadius);
        var randomZ = Random.Range(-spawnRadius, spawnRadius);

        collectableInstance.Dead += HandleCollectableDead;

        var position = new Vector3(transform.position.x + randomX, 0, transform.position.z + randomZ);
        collectableInstance.transform.position = position;
    }

    private void HandleCollectableDead()
    {
        SpawnCollectable();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, spawnRadius * 2);
    }
}