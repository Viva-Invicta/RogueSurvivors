using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class UnitAnimationController : MonoBehaviour, IInitializableWithUnitStatusComponent
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

        //todo move to unit status
        private bool isWalking;

        public void Initialize(IUnitStatusProvider unitStatus)
        {
            SetMovementSpeed(unitStatus.BaseMovementSpeed);
        }

        public void SetAnimationByKey(AnimatorKeyIdentifier key, bool isActive)
        {
            if (AnimatorKeysUtility.TryGetAnimatorHash(key, out var hash))
            {
                animator.SetBool(hash, isActive);
            }
        }

        public void TriggerAttackAnimation()
        {
            SetWalking(false);

            var animationIndex = Random.Range(0, attackAnimations.Count);
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

        public void SetPreview(bool isPreview)
        {
            if(AnimatorKeysUtility.TryGetAnimatorHash(AnimatorKeyIdentifier.Preview, out var hash))
            {
                animator.SetBool(hash, isPreview);
            }
        }

        public void SetAttackSpeed(float attackSpeed)
        {
            if (AnimatorKeysUtility.TryGetAnimatorHash(AnimatorKeyIdentifier.MoveSpeed, out var hash))
            {
                animator.SetFloat(hash, attackSpeed);
            }
        }

        public void SetMovementSpeed(float movementSpeed)
        {
            if (AnimatorKeysUtility.TryGetAnimatorHash(AnimatorKeyIdentifier.MoveSpeed, out var hash))
            {
                animator.SetFloat(hash, movementSpeed);
            }
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