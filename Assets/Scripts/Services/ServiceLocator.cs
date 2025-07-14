using UnityEngine;

namespace DunDungeons
{
    public class ServiceLocator : MonoBehaviour
    {
        [field: SerializeField] public InputService InputService { get; private set; }
        [field: SerializeField] public UIService UIService { get; private set; }
        [field: SerializeField] public EntitiesService EntitiesService { get; private set; }
        [field: SerializeField] public SpawnerService SpawnerService { get; private set; }
        [field: SerializeField] public PrefabsService PrefabsService { get; private set; }
    }
}
