using System;
using System.Collections;
using UnityEngine;

namespace AutoBattler
{
    public class UnitCombatController : MonoBehaviour, IInitializableWithUnitStatusComponent
    {
        public event Action StartedAttack;
        public event Action<float> StartedCooldown;
        public event Action EndedCooldown;

        [SerializeField] private float delayBeforeWeaponActivation = 0.2f; // in percent
        [SerializeField] private float earlyWeaponDeactivation = 0.2f;     // in percent
        [SerializeField] private UnitMeleeWeapon meleeWeapon;

        protected ServiceLocator ServiceLocator { get; private set; }

        protected bool isInCooldown;
        private float actualCooldown;

        private IUnitStatusProvider unitStatus;

        public void Initialize(IUnitStatusProvider unitStatus)
        {
            this.unitStatus = unitStatus;
        }

        public void Attack(UnitBehaviourController target)
        {
            if (unitStatus.IsAttackInCooldown || unitStatus.State != UnitState.Fight || unitStatus.IsAttackLocked)
            {
                return;
            }

            actualCooldown = unitStatus.UnitValuesCalculator.CalculateAttackCooldown();
            StartCoroutine(WaitForWeaponActivation());
            StartCoroutine(Cooldown());
        }

        protected IEnumerator WaitForWeaponActivation()
        {
            yield return new WaitForSeconds(actualCooldown * delayBeforeWeaponActivation);

            meleeWeapon.Activate();
            StartedAttack?.Invoke();
        }

        protected IEnumerator Cooldown()
        {
            StartedCooldown?.Invoke(actualCooldown);

            var deactivateDelay = actualCooldown * (1f - earlyWeaponDeactivation);
            yield return new WaitForSeconds(deactivateDelay);

            meleeWeapon.Deactivate();

            var remainingCooldown = actualCooldown - deactivateDelay;
            if (remainingCooldown > 0f)
            {
                yield return new WaitForSeconds(remainingCooldown);
            }

            EndedCooldown?.Invoke();
        }
    }
}
