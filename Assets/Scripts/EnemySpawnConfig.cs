using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DunDungeons
{
    [Serializable]
    public class EnemySpawnConfig
    {
        [Serializable]
        public class EnemySpawnEntry
        {
            public CharacterType CharacterType;

            [Min(0)]
            public int Weight = 1;
        }

        [field: SerializeField]
        public float SpawnRadius { get; private set; } = 3f;

        [field: SerializeField]
        [SceneObjectsOnly]
        public List<Transform> SpawnPoints { get; private set; }

        [field: SerializeField]
        public List<EnemySpawnEntry> Enemies { get; private set; } = new List<EnemySpawnEntry>
        {
            new EnemySpawnEntry { CharacterType = CharacterType.Skeleton, Weight = 7 },
            new EnemySpawnEntry { CharacterType = CharacterType.FastSkeleton, Weight = 2 },
            new EnemySpawnEntry { CharacterType = CharacterType.BigSkeleton, Weight = 1 }
        };

        public CharacterType GetRandomCharacterType()
        {
            var totalWeight = Enemies.Sum(enemy => enemy.Weight);
            var randomValue = Random.Range(0, totalWeight);
            var current = 0;

            foreach (var entry in Enemies)
            {
                current += entry.Weight;
                if (randomValue < current)
                {
                    return entry.CharacterType;
                }
            }

            return Enemies.First().CharacterType;
        }
    }

}
