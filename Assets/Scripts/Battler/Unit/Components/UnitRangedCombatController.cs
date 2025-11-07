using System.Collections;
using UnityEngine;

namespace AutoBattler
{
    public class UnitRangedCombatController : UnitCombatControllerBase
    {
        [SerializeField] private float projectileSpawnDelay = 0.3f;

        protected override void OnAfterAttackStarted()
        {
            base.OnAfterAttackStarted();
            StartCoroutine(ShootWithDelay());
        }

        private IEnumerator ShootWithDelay()
        {
            yield return new WaitForSeconds(projectileSpawnDelay);

            if (Weapon)
            {
                Weapon.Activate();
            }
        }
    }
}
