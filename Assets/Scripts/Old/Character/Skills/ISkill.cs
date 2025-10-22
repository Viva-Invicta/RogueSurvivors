using System;
using UnityEngine;

namespace DunDungeons
{
    public interface ISkill
    {
        event Action Completed;

        SkillConfiguration Configuration { get; }

        void Activate(Transform caster, float duration);
        void Process(float deltaTime);
    }
}