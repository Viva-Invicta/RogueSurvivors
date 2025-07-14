using System;
using UnityEngine;

namespace DunDungeons
{
    public class HealthComponent : MonoBehaviour
    {
        public event Action Updated;

        [SerializeField]
        private int maxHP = 100;

        private int currentHP;

        public int MaxHP => maxHP;
        public int CurrentHP => currentHP;

        private void Awake()
        {
            currentHP = maxHP;
        }

        public void ConsumeHP(int amount)
        {
            currentHP -= amount;
            Updated?.Invoke();
        }
    }
}
