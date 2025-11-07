using System;
using System.Collections;
using UnityEngine;

namespace AutoBattler
{
    public abstract class UnitCombatControllerBase : MonoBehaviour, IInitializableWithUnitStatusComponent
    {
        public event Action StartedAttack;
        public event Action<float> StartedCooldown;
        public event Action EndedCooldown;

        protected ServiceLocator ServiceLocator { get; private set; }

        protected bool isInCooldown;
        protected float cooldown;

        private IUnitStatusProvider unitStatus;
        protected UnitWeapon Weapon;

        public void Initialize(IUnitStatusProvider unitStatus)
        {
            this.unitStatus = unitStatus;
            Weapon = unitStatus.Weapon;
        }

        public void Attack(UnitBehaviourController target)
        {
            cooldown = unitStatus.UnitValuesCalculator.CalculateAttackCooldown();
            Weapon.SetTarget(target);

            OnAfterAttackStarted();
        }

        protected virtual void OnAfterAttackStarted()
        {
            StartedAttack?.Invoke();
            StartCoroutine(Cooldown());
        }

        protected IEnumerator Cooldown()
        {
            StartedCooldown?.Invoke(cooldown);
            
            yield return new WaitForSeconds(cooldown);

            EndedCooldown?.Invoke();
        }
    }
}
