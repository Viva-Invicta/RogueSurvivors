using UnityEngine;

namespace DunDungeons
{
    public class PlayerBehaviourController : CharacterBehaviourController
    {
        protected override void OnAfterInitialize()
        {
            inputService = serviceLocator.InputService;
            isInitialized = true;

            movementController.DashStarted += HandleDashStarted;
            movementController.DashCompleted += HandleDashCompleted;
            movementController.DashCooldownCompleted += HandleDashCooldownCompleted;

            dashBar = serviceLocator.UIService.AddDashBar(gameObject, movementController, healthComponent);
            dashBar.gameObject.SetActive(false);
        }

        private InputService inputService;
        private bool isInitialized;
        private PlayerDashBar dashBar;

        private void FixedUpdate()
        {
            if (!isInitialized)
            {
                return;
            }

            if (inputService.AttackPressed)
            {
                combatController.Attack();
            }

            if (inputService.Special1Pressed && !State.IsDashInCooldown)
            {
                movementController.PerformDash();
                return;
            }

            movementController.Move(inputService.InputDirection);

            if (inputService.InputDirection.magnitude > 0)
            {
                animationController.SetWalking(true);
            }
            else
            {
                animationController.SetWalking(false);
            }
        }

        private void HandleDashStarted()
        {
            State.IsMovementLocked = true;
            State.IsAttackLocked = true;
            State.IsDashInCooldown = true;
            dashBar.gameObject.SetActive(true);
            animationController.PlayDash();
            soundController.PlayDashClip();
            effectsController.PlayDashEffect(serviceLocator.InputService.InputDirection);
        }

        private void HandleDashCompleted()
        {
            State.IsMovementLocked = false;
            State.IsAttackLocked = false;
        } 

        private void HandleDashCooldownCompleted()
        {
            if (dashBar)
            {
                dashBar.gameObject.SetActive(false);
            }
            State.IsDashInCooldown = false;
        }

        protected override void OnAfterDestroy()
        {
            movementController.DashStarted -= HandleDashStarted;
            movementController.DashCompleted -= HandleDashCompleted;
            movementController.DashCooldownCompleted -= HandleDashCooldownCompleted;
        }
    }
}