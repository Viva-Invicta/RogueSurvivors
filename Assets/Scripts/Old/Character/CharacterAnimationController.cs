using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterAnimationController : MonoBehaviour, IInitializableCharacterComponent
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private List<AnimatorKeyIdentifier> attackAnimations = new List<AnimatorKeyIdentifier>()
        {
            AnimatorKeyIdentifier.Attack1,
            AnimatorKeyIdentifier.Attack2,
            AnimatorKeyIdentifier.Attack3
        };

        private const string WalkSpeedAnimatorParamater = "Speed";
        private const string AttackSpeedAnimatorParameter = "AttackSpeed";


        protected ICharacterStateProvider characterState;
        protected ServiceLocator ServiceLocator { get; private set; }

        private bool isWalking;

        public void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
            ServiceLocator = serviceLocator;
            characterState = state;
            SetMovementSpeed(state.MovementSpeed);
        }

        public void SetAnimationByKey(AnimatorKeyIdentifier key, bool isActive)
        {
            if (AnimatorKeysUtility.TryGetAnimatorHash(key, out var hash))
            {
                animator.SetBool(hash, true);
            }
        }

        public void TriggerAttackAnimation()
        {
            SetWalking(false);

            var animationIndex = UnityEngine.Random.Range(0, attackAnimations.Count);
            var animationId = attackAnimations.ElementAt(animationIndex);

            if (AnimatorKeysUtility.TryGetAnimatorHash(animationId, out var hash))
            {
                animator.SetTrigger(hash);
            }

            SetWalking(false);
        }

        public void SetWalking(bool isWalking)
        {
            if (this.isWalking != isWalking)
            {
                this.isWalking = isWalking;
                if (AnimatorKeysUtility.TryGetAnimatorHash(AnimatorKeyIdentifier.Walking, out var hash))
                {
                    animator.SetBool(hash, isWalking);
                }
            }
        }

        public void SetAttackSpeed(float attackSpeed)
        {
            animator.SetFloat(AttackSpeedAnimatorParameter, attackSpeed);
        }

        public void SetMovementSpeed(float movementSpeed)
        {
            animator.SetFloat(WalkSpeedAnimatorParamater, movementSpeed);
        }

        public void PlayDash()
        {
            SetWalking(false);
            if (AnimatorKeysUtility.TryGetAnimatorHash(AnimatorKeyIdentifier.Dash, out var hash))
            {
                animator.SetTrigger(hash);
            }
        }

        public void PlayDeath()
        {
            SetWalking(false);
            if (AnimatorKeysUtility.TryGetAnimatorHash(AnimatorKeyIdentifier.Death, out var hash))
            {
                animator.SetTrigger(hash);
            }
        }

        public void PlayCheer()
        {
            SetWalking(false);
            if (AnimatorKeysUtility.TryGetAnimatorHash(AnimatorKeyIdentifier.Cheer, out var hash))
            {
                animator.SetBool(hash, true);
            }
        }
    }
}
