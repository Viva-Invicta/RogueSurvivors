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
        private HealthComponent healthComponent;

        [SerializeField]
        private float delayBeforeDeath = 5f;

        [SerializeField]
        private GameObject deathEffect;

        [SerializeField]
        private GameObject spawnEffect;

        public bool IsDead { get; private set; }

        protected virtual void OnEnable()
        {
            healthComponent.Updated += HandleHPUpdated;    
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

        private void HandleHPUpdated()
        {
            if (healthComponent.CurrentHP <= 0)
            {
                IsDead = true;
                characterAnimationController.PlayDeath();

                if (deathEffect)
                {
                    deathEffect.SetActive(true);
                }

                Destroy(gameObject, delayBeforeDeath);
            }
        }
    }
}