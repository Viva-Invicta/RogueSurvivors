using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class EnemyFormationConfigsService : MonoBehaviour
    {
        [SerializeField]
        private List<EnemyFormationConfig> formationConfigurations;

        public EnemyFormationConfig GetFormationConfigForRoom(int roomWidth, int roomHeight)
        {
            var formation = formationConfigurations.FirstOrDefault(configuration =>
            {
                return configuration.RoomSize.x == roomWidth && configuration.RoomSize.y == roomHeight;
            });

            if (formation == default)
            {
                Debug.LogError($"{nameof(EnemyFormationConfigsService)} : Can't find formation config for room size {roomWidth} {roomHeight}");
            }

            return formation;
        }
    }
}