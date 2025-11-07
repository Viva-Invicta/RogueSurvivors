using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitRangedWeapon : UnitWeapon
    {
        [SerializeField] private float delayBeforeHitDestroy = 5f;
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private GameObject projectileHitPrefab;
        [SerializeField] private Transform spawnPoint;

        private UnitBehaviourController owner;
        private IEnumerable<DamageType> damageTypes;

        public override void Initialize(UnitBehaviourController owner, IEnumerable<DamageType> damageType)
        {
            this.owner = owner;
            damageTypes = damageType;
        }

        public override void Activate()
        {
            Fire(spawnPoint);
        }

        private void Fire(Transform spawnPoint)
        {
            if (!projectilePrefab || !spawnPoint || !ActiveTarget)
            {
                return;
            }

            var projectile = Object.Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            projectile.Initialize(ActiveTarget);
            projectile.TargetAchieved += () => HandleTargetAchieved(projectile);
            projectile.StartCast();

        }

        private void HandleTargetAchieved(Projectile projectile)
        {
            ApplyDamage(projectile.Target);
            projectile.Release();
            Destroy(projectile.gameObject);
            var projectileHit = Instantiate(projectileHitPrefab, projectile.transform.position, Quaternion.identity);
            StartCoroutine(WaitForProjectileHitDestroy(projectileHit));
        }

        private void ApplyDamage(UnitBehaviourController target)
        {
            var ownerStatusProvider = owner.StatusProvider;

            var damageReceiver = target.ComponentsContainer.DamageReceiver;
            var ownerValuesCalculator = ownerStatusProvider.UnitValuesCalculator;

            foreach (var damageType in damageTypes)
            {
                var damage = ownerValuesCalculator.CalculateOutcomingDamage(damageType);
                damageReceiver.ReceiveDamage(damageType, damage);
            }
        }

        private IEnumerator WaitForProjectileHitDestroy(GameObject projectileHit)
        {
            yield return new WaitForSecondsRealtime(delayBeforeHitDestroy);

            Destroy(projectileHit);
        }
    }
}