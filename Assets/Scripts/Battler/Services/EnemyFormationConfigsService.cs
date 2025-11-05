using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class EnemyFormationConfigsService : MonoBehaviour
    {
        [SerializeField]
        private List<EnemyFormationConfig> formationConfigurations;

        public EnemyFormationConfig CurrentEnemiesFormation { get; private set; }

        public void UpdateFormationConfig(int roomWidth, int roomHeight)
        {
            CurrentEnemiesFormation = formationConfigurations.FirstOrDefault(configuration =>
               configuration.RoomSize.x == roomWidth && configuration.RoomSize.y == roomHeight
            );

            if (CurrentEnemiesFormation == default)
            {
                Debug.LogError($"{nameof(EnemyFormationConfigsService)} : Can't find formation config for room size {roomWidth} {roomHeight}");
            }
        }
    }
}