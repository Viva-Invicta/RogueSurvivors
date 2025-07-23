using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public abstract class CharacterBehaviourController : MonoBehaviour, IHaveFaction
    {
        [field: SerializeField]
        public Faction Faction { get; private set; } = Faction.Enemy;

        [field: SerializeField]
        public CharacterType CharacterType { get; private set; }

        [SerializeField]
        protected HealthComponent healthComponent;

        [SerializeField]
        private float delayBeforeDeath = 5f;

        [SerializeField] protected CharacterCombatController combatController;
        [SerializeField] protected CharacterMovementController movementController;
        [SerializeField] protected CharacterAnimationController animationController;
        [SerializeField] protected CharacterEffectsController effectsController;
        [SerializeField] protected CharacterSoundController soundController;

        protected ServiceLocator serviceLocator;

        private int lastHP;
        private CharacterState state;

        protected CharacterState State => state;

        private void Start()
        {
            effectsController.PlaySpawnEffect();
        }

        public void Initialize(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;

            state = new CharacterState();
            state.RootComponent = this;
            state.MovementSpeed = movementController.MovementSpeed;

            lastHP = healthComponent.MaxHP;

            healthComponent.Updated += HandleHPUpdated;
            combatController.StartedAttack += HandleAttackStarted;
            combatController.StartedCooldown += HandleWeaponCooldownStarted;
            combatController.EndedCooldown += HandleWeaponCooldownEnded;

            var controllers = new List<MonoBehaviour> 
            { 
                combatController,
                movementController, 
                animationController, 
                effectsController, 
                soundController 
            };

            foreach (var controller in controllers)
            {
                InitializeController(controller);
            }

            OnAfterInitialize();
        }

        private void OnDestroy()
        {
            OnAfterDestroy();
        }

        protected abstract void OnAfterInitialize();
        protected abstract void OnAfterDestroy();

        private void InitializeController(MonoBehaviour controller)
        {
            if (controller is IInitializableCharacterComponent initializableComponent)
            {
                initializableComponent.Initialize(serviceLocator, state);
            }
        }

        private void HandleAttackStarted()
        {
            soundController.PlayAttackClip();
        }

        private void HandleHPUpdated()
        {
            var currentHP = healthComponent.CurrentHP;
            if (currentHP > 0 && currentHP < lastHP)
            {
                soundController.PlayDamagedClip();
            }

            if (currentHP <= 0)
            {
                soundController.PlayDeathClip();

                state.IsDead = true;

                animationController.PlayDeath();
                effectsController.PlayDeathEffect(delayBeforeDeath);

                Destroy(gameObject, delayBeforeDeath);
            }

            lastHP = currentHP;
        }

        private void HandleWeaponCooldownStarted(float cooldown)
        {
            animationController.SetAttackSpeed(1 / cooldown);
            animationController.TriggerAttackAnimation();
            state.IsWeaponInCooldown = true;
            state.IsMovementLocked = true;
        }

        private void HandleWeaponCooldownEnded()
        {
            state.IsWeaponInCooldown = false;
            state.IsMovementLocked = false;
        }
    }
}