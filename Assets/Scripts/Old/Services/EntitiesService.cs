using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public class EntitiesService : MonoBehaviour
    {
        public GameObject Player { get; private set; }
        public IEnumerable<GameObject> Enemies => enemies;

        private List<GameObject> enemies = new List<GameObject>();

        public void SetPlayer(GameObject player)
        {
            Player = player;
        }

        public void AddEnemy(GameObject enemy)
        {
            enemies.Add(enemy);
        }

        public void RemoveEnemy(GameObject enemy)
        {
            if (enemies.Contains(enemy))
            {
                enemies.Remove(enemy);
            }
        }
    }
}
