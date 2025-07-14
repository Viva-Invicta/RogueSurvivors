using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public class EntitiesService : MonoBehaviour
    {
        public GameObject Player { get; private set; }
        public List<GameObject> Enemies { get; } = new List<GameObject>();

        public void SetPlayer(GameObject player)
        {
            Player = player;
        }

        public void AddEnemy(GameObject enemy)
        {
            Enemies.Add(enemy);
        }
    }
}
