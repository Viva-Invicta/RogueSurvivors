using System;
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace DunDungeons
{
    public class CharacterCombatController : MonoBehaviour, IInitializableCharacterComponent
    {
        public event Action StartedAttack;
        public event Action<float> StartedCooldown;
        public event Action EndedCooldown;

        [SerializeField] protected float attackCooldown = 1f;
        [SerializeField] private Weapon weapon;
        [SerializeField] private float delayBeforeWeaponActivation = 0.2f; //in percent

        protected ServiceLocator ServiceLocator { get; private set; }
        protected ICharacterStateProvider CharacterState { get; private set; }

        protected bool isInCooldown;

        public void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
            ServiceLocator = serviceLocator;
            CharacterState = state;
        }

        public void Attack()
        {
            if (CharacterState.IsWeaponInCooldown || CharacterState.IsDead || CharacterState.IsAttackLocked)
            {
                return;
            }

            CharacterState.RootComponent.StartCoroutine(WaitForWeaponActivation());
            CharacterState.RootComponent.StartCoroutine(Cooldown());
        }

        protected IEnumerator WaitForWeaponActivation()
        {
            yield return new WaitForSeconds(attackCooldown * delayBeforeWeaponActivation);

            weapon.Activate();
            StartedAttack?.Invoke();
        }

        protected IEnumerator Cooldown()
        {
            StartedCooldown?.Invoke(attackCooldown);

            yield return new WaitForSeconds(attackCooldown);

            weapon.Deactivate();
            EndedCooldown?.Invoke();
        }
    }
}