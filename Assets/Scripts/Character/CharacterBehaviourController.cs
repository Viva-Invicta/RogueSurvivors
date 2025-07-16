using DG.Tweening;
using System.Linq;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterBehaviourController : MonoBehaviour, IHaveFaction
    {
        [field: SerializeField]
        public Faction Faction { get; private set; } = Faction.Enemy;

        [field: SerializeField]
        public CharacterType CharacterType { get; private set; }

        [SerializeField]
        private CharacterAnimationController characterAnimationController;

        [SerializeField]
        private CharacterCombatController characterCombatController;

        [SerializeField]
        private HealthComponent healthComponent;

        [SerializeField]
        private float delayBeforeDeath = 5f;

        [SerializeField]
        private GameObject deathEffect;

        [SerializeField]
        private GameObject spawnEffect;

        [SerializeField]
        private AudioClip[] damageAudioClips;

        [SerializeField]
        private AudioClip[] attackAudioClips;

        [SerializeField]
        private AudioClip deathSound;

        [SerializeField]
        private AudioSource audioSource;

        private int lastHP;

        public bool IsDead { get; private set; }

        protected virtual void OnEnable()
        {
            healthComponent.Updated += HandleHPUpdated;

            characterCombatController.StartedAttack += HandleAttackStarted;
            lastHP = healthComponent.MaxHP;
        }

        private void Start()
        {
            if (spawnEffect)
            {
                spawnEffect.SetActive(true);
            }
        }

        public virtual void Initialize(ServiceLocator serviceLocator)
        {

        }

        private void HandleAttackStarted()
        {
            if (audioSource && attackAudioClips.Any())
            {
                var attackAudioClip = attackAudioClips[Random.Range(0, attackAudioClips.Length)];
                audioSource.PlayOneShot(attackAudioClip);
            }
        }

        private void HandleHPUpdated()
        {
            var currentHP = healthComponent.CurrentHP;
            if (currentHP > 0 && currentHP < lastHP && audioSource && damageAudioClips.Any())
            {
                var damageAudioClip = damageAudioClips[Random.Range(0, damageAudioClips.Length)];
                audioSource.PlayOneShot(damageAudioClip);
            }

            if (currentHP <= 0)
            {   
                if (deathSound && audioSource)
                {
                    audioSource.PlayOneShot(deathSound);
                }

                IsDead = true;
                characterAnimationController.PlayDeath();

                if (deathEffect)
                {
                    deathEffect.SetActive(true);
                }

                transform.DOScale(0, delayBeforeDeath).SetEase(Ease.InBack);
                Destroy(gameObject, delayBeforeDeath);
            }
        }
    }
}