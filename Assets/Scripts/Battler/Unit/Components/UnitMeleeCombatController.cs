using System.Collections;
using UnityEngine;

namespace AutoBattler
{
    public class UnitMeleeCombatController : UnitCombatControllerBase
    {
        [SerializeField] private float delayBeforeWeaponActivation = 0.2f; // in percent
        [SerializeField] private float earlyWeaponDeactivation = 0.2f;     // in percent

        protected override void OnAfterAttackStarted()
        {
            base.OnAfterAttackStarted();
            StartCoroutine(WaitForWeaponActivation());
            StartCoroutine(WaitForWeaponDeactivation());
        }

        private IEnumerator WaitForWeaponActivation()
        {
            yield return new WaitForSeconds(cooldown * delayBeforeWeaponActivation);

            Weapon.Activate();
        }

        private IEnumerator WaitForWeaponDeactivation()
        {
            var deactivateDelay = cooldown * (1f - earlyWeaponDeactivation);

            yield return new WaitForSeconds(deactivateDelay);

            Weapon.Deactivate();
        }
    }
}
