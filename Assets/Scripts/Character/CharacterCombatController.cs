using System.Collections;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterCombatController : MonoBehaviour
    {
        [SerializeField] protected float attackCooldown = 1f;
        [SerializeField] private Weapon weapon;
        [SerializeField] private float delayBeforeWeaponActivation = 0.2f; //in percent

        protected CharacterAnimationController animationController;
        protected CharacterMovementController movementController;
        protected CharacterBehaviourController behaviourController;

        protected bool isInCooldown;

        private void OnEnable()
        {
            animationController = GetComponent<CharacterAnimationController>();

            if (!animationController)
            {
                Debug.LogError("No animation controller on " + gameObject.name);
            }

            movementController = GetComponent<CharacterMovementController>();

            if (!movementController)
            {
                Debug.LogError("No movement controller on " + gameObject.name);
            }

            behaviourController = GetComponent<CharacterBehaviourController>();
        }

        public void Attack()
        {
            if (isInCooldown || behaviourController.IsDead)
            {
                return;
            }

            isInCooldown = true;

            StartCoroutine(WaitForWeaponActivation());

            animationController.SetAttackSpeed(1 / attackCooldown);
            animationController.TriggerAttackAnimation();
            movementController.LockMovement(isLocked: true);

            StartCoroutine(Cooldown());
        }

        protected IEnumerator WaitForWeaponActivation()
        {
            yield return new WaitForSecondsRealtime(attackCooldown * delayBeforeWeaponActivation);
            weapon.Activate();
        }

        protected IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            weapon.Deactivate();
            movementController.LockMovement(isLocked: false);
            isInCooldown = false;
        }
    }
}