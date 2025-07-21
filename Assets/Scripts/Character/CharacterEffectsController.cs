using DG.Tweening;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterEffectsController : MonoBehaviour
    {
        [SerializeField]
        private GameObject deathEffect;

        [SerializeField]
        private GameObject spawnEffect;

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
    }
}