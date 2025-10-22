using System;
using UnityEngine;

namespace DunDungeons
{
    public class JuggernautSkill : ISkill
    {
        public event Action Completed;

        private float passedTime;
        private float duration;

        private bool isStarted;

        public SkillConfiguration Configuration { get; private set; }

        public JuggernautSkill(SkillConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Activate(Transform caster, float duration)
        {
            passedTime = 0f;
            isStarted = true;
        }

        public void Process(float deltaTime)
        {
            if (!isStarted)
            {
                return;
            }

            passedTime += deltaTime;
            if (passedTime >= duration)
            {
                HandleComplete();
            }
        }
        
        private void HandleComplete()
        {
            isStarted = false;
            Completed?.Invoke();
        }
    }
}