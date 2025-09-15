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
        [SerializeField] private float delayBeforeWeaponActivation = 0.2f; //in percent

        protected ServiceLocator ServiceLocator { get; private set; }
        protected ICharacterStateProvider CharacterState { get; private set; }
        private float AttackCooldown => initialAttackCooldown * attackCooldownModifier.Value;

        protected bool isInCooldown;

        private Modifier attackCooldownModifier;

        public void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
            ServiceLocator = serviceLocator;
            CharacterState = state;

            attackCooldownModifier = serviceLocator.ModifiersService.GetModifier(state.Faction, ModifierType.AttackCooldown);
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

            yield return new WaitForSeconds(AttackCooldown);

            weapon.Deactivate();
            EndedCooldown?.Invoke();
        }
    }
}