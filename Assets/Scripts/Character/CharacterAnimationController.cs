using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterAnimationController : MonoBehaviour
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

        private const string WalkingAnimation = "Walking";
        private const string AttackSpeedAnimatorParameter = "AttackSpeed";
        private const string DeathAnimation = "Death";

        private CharacterController characterController;
        private bool isWalking;

        private void OnEnable()
        {
            characterController = GetComponent<CharacterController>();

            if (!characterController)
            {
                Debug.LogError("No character controller on " + gameObject.name);
            }
        }

        public void TriggerAttackAnimation()
        {
            SetWalking(false);

            var animationIndex = Random.Range(0, attackAnimationsList.Count);
            var animationName = attackAnimationsList.ElementAt(animationIndex);

            animator.SetTrigger(animationName);
            SetWalking(false);
            //animator.ResetTrigger(animationName);
        }

        public void SetWalking(bool isWalking)
        {
            if (this.isWalking != isWalking)
            {
                this.isWalking = isWalking;
                animator.SetBool(WalkingAnimation, isWalking);
            }
        }

        public void SetAttackSpeed(float attackSpeed)
        {
            animator.SetFloat(AttackSpeedAnimatorParameter, attackSpeed);
        }

        public void PlayDeath()
        {
            SetWalking(false);
            animator.SetTrigger(DeathAnimation);
        }
    }
}