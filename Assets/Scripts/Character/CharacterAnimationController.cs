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
        private List<string> attackAnimationsList = new List<string>()
        {
            "Attack1",
            "Attack2",
            "Attack3"
        };

        private const string DashAnimation = "Dash";
        private const string WalkAnimation = "Walking";
        private const string WalkSpeedAnimatorParamater = "Speed";
        private const string AttackSpeedAnimatorParameter = "AttackSpeed";
        private const string DeathAnimation = "Death";

        private const float MinSpeedToRunAnimation = 2.5f;

        protected ICharacterStateProvider characterState;
        protected ServiceLocator ServiceLocator { get; private set; }

        private bool isWalking;

        public void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
            ServiceLocator = serviceLocator;
            characterState = state;
            SetMovementSpeed(state.MovementSpeed);
        }

        public void TriggerAttackAnimation()
        {
            SetWalking(false);

            var animationIndex = UnityEngine.Random.Range(0, attackAnimationsList.Count);
            var animationName = attackAnimationsList.ElementAt(animationIndex);

            animator.SetTrigger(animationName);
            SetWalking(false);
        }

        public void SetWalking(bool isWalking)
        {
            if (this.isWalking != isWalking)
            {
                this.isWalking = isWalking;
                
                animator.SetBool(WalkAnimation, isWalking);
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
            animator.SetTrigger(DashAnimation);
        }

        public void PlayDeath()
        {
            SetWalking(false);
            animator.SetTrigger(DeathAnimation);
        }
    }
}