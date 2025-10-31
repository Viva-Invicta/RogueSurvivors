using System;

namespace AutoBattler
{
    public class Resource
    {
        public event Action Updated;

        private readonly float maxValue;
        private float currentValue;

        public float MaxValue => maxValue;
        public float CurrentValue => currentValue;

        public Resource(float maxValue)
        {
            this.maxValue = maxValue;
            currentValue = maxValue;
        }

        public void Consume(float amount)
        {
            currentValue -= amount;
            Updated?.Invoke();
        }

        public void Add(float amount)
        {
            currentValue += amount;
            Updated?.Invoke();
        }

        public void Reset()
        {
            currentValue = maxValue;
            Updated?.Invoke();
        }
    }
}