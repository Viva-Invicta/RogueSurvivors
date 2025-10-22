using System;
using System.Collections;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterCombatController : MonoBehaviour, IInitializableCharacterComponent
    {
        public event Action StartedAttack;
        public event Action<float> StartedCooldown;
        public event Action EndedCooldown;

        [SerializeField] protected float initialAttackCooldown = 1f;
        [SerializeField] private Weapon weapon;
        [SerializeField] private float delayBeforeWeaponActivation = 0.2f; // in percent
        [SerializeField] private float earlyWeaponDeactivation = 0.2f;     // in percent

        protected ServiceLocator ServiceLocator { get; private set; }
        protected ICharacterStateProvider CharacterState { get; private set; }
        private float AttackCooldown => initialAttackCooldown * attackCooldownModifier.Value;

        protected bool isInCooldown;

        private Modifier weaponSizeModifier;
        private Modifier attackCooldownModifier;

        public void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
            ServiceLocator = serviceLocator;
            CharacterState = state;

            var modifierService = serviceLocator.ModifiersService;
            attackCooldownModifier = modifierService.GetModifier(state.Faction, ModifierType.AttackCooldown);
            weaponSizeModifier = modifierService.GetModifier(state.Faction, ModifierType.WeaponSize);

            weapon.SetScale(weaponSizeModifier.Value);

            weaponSizeModifier.Updated += HandleWeaponSizeModifierUpdated;
        }

        public void Attack()
        {
            if (CharacterState.IsWeaponInCooldown || CharacterState.IsDead || CharacterState.IsAttackLocked)
            {
                return;
            }

            StartCoroutine(WaitForWeaponActivation());
            StartCoroutine(Cooldown());
        }

        protected IEnumerator WaitForWeaponActivation()
        {
            yield return new WaitForSeconds(AttackCooldown * delayBeforeWeaponActivation);

            weapon.Activate();
            StartedAttack?.Invoke();
        }

        protected IEnumerator Cooldown()
        {
            StartedCooldown?.Invoke(AttackCooldown);

            var deactivateDelay = AttackCooldown * (1f - earlyWeaponDeactivation);
            yield return new WaitForSeconds(deactivateDelay);

            weapon.Deactivate();

            var remainingCooldown = AttackCooldown - deactivateDelay;
            if (remainingCooldown > 0f)
                yield return new WaitForSeconds(remainingCooldown);

            EndedCooldown?.Invoke();
        }

        private void HandleWeaponSizeModifierUpdated()
        {
            weapon.SetScale(weaponSizeModifier.Value);
        }
    }
}
