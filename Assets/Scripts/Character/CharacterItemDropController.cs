using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterItemDropController : MonoBehaviour, IInitializableCharacterComponent
    {
        [SerializeField]
        private DropProbabilitySettings[] dropSettings;

        private ICharacterStateProvider state;

        public void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
            this.state = state;
        }

        public void TryDropItems()
        {
            var random = UnityEngine.Random.Range(0f, 1f);
            var cumulative = 0f;

            foreach (var setting in dropSettings)
            {
                cumulative += setting.Probability;

                if (random <= cumulative)
                {
                    var droppedItem = Instantiate(setting.ItemPrefab);
                    droppedItem.transform.position = state.RootComponent.transform.position;
                    return;
                }
            }
        }
    }

    [Serializable]
    public struct DropProbabilitySettings
    {
        [field: SerializeField]
        [AssetsOnly]
        public PickupItem ItemPrefab { get; private set; }

        [field: SerializeField]
        public float Probability { get; private set; }


    }
}