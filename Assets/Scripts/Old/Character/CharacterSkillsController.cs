using System;
using System.Collections;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterSkillsController : MonoBehaviour, IInitializableCharacterComponent
    {
        public event Action SkillUseCompleted;

        public ISkill ActiveSkill { get; private set; }

        private bool IsInCooldown;

        public void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
        }

        public void SetActiveSkill(SkillConfiguration skillConfiguration)
        {
            if (SkillsUtility.TryGetSkillByConfiguration(skillConfiguration, out var ActiveSkill))
            {
                this.ActiveSkill = ActiveSkill;
                ActiveSkill.Completed += HandleSkillUsageCompleted;
            }
        }

        private void Update()
        {
            ActiveSkill?.Process(Time.deltaTime);
        }

        public bool TryUseSkill()
        {
            if (ActiveSkill != null && !IsInCooldown)
            {
                ActiveSkill.Activate(transform, ActiveSkill.Configuration.Duration);
                IsInCooldown = true;

                return true;
            }

            return false;
        }

        private void HandleSkillUsageCompleted()
        {
            StartCoroutine(Cooldown());
            SkillUseCompleted?.Invoke();
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSecondsRealtime(ActiveSkill.Configuration.InitialCooldown);
            IsInCooldown = false;
        }
    }
}