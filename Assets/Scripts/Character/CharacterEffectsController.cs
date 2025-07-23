using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterEffectsController : MonoBehaviour
    {
        [SerializeField]
        private GameObject deathEffect;

        [SerializeField]
        private GameObject spawnEffect;

        [SerializeField]
        private GameObject dashEffectPrefab;

        public void PlaySpawnEffect()
        {
            if (spawnEffect)
            {
                spawnEffect.SetActive(true);
            }
        }

        public void PlayDeathEffect(float delayBeforeDeath)
        {
            if (deathEffect)
            {
                deathEffect.SetActive(true);
            }

            transform.DOScale(0, delayBeforeDeath).SetEase(Ease.InBack);
        }

        public void PlayDashEffect(Vector3 lookDirection)
        {
            if (dashEffectPrefab)
            {
                var dashEffect = Instantiate(dashEffectPrefab);
                dashEffect.transform.position = transform.position;

                var lookRotation = Quaternion.LookRotation(-lookDirection);
                dashEffect.transform.rotation = lookRotation;
                Destroy(dashEffect, 5f);
            }
        }
    }
}