using UnityEngine;

namespace DunDungeons
{
    public class PlayerBehaviourController : CharacterBehaviourController
    {
        protected override void OnAfterInitialize()
        {
            inputService = serviceLocator.InputService;
            isInitialized = true;
        }

        private InputService inputService;
        private bool isInitialized;

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

            if (State.IsMovementLocked)
            {
                return;
            }

            movementController.Move(direction: CalculateDirection());
        }

        private Vector3 CalculateDirection()
        {
            var xInput = 0;
            var zInput = 0;

            if (inputService.UpPressed)
            {
                zInput += 1;
            }

            if (inputService.DownPressed)
            {
                zInput -= 1;
            }

            if (inputService.LeftPressed)
            {
                xInput -= 1;
            }

            if (inputService.RightPressed)
            {
                xInput += 1;
            }

            if (xInput != 0 || zInput != 0)
            {
                animationController.SetWalking(true);
            }
            else
            {
                animationController.SetWalking(false);
            }

            return new Vector3(xInput, 0, zInput).normalized;
        }
    }
}