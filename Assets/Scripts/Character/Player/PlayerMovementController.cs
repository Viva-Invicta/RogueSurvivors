using UnityEngine;

namespace DunDungeons
{
    public class PlayerMovementController : CharacterMovementController
    {
        private CharacterAnimationController characterAnimationController;
        private InputService inputService;
        private bool isInitialized;

        protected override void OnEnable()
        {
            base.OnEnable();

            characterAnimationController = GetComponent<CharacterAnimationController>();
        }

        private void FixedUpdate()
        {
            if (!isInitialized)
            {
                return;
            }

            if (isMovementLocked)
            {
                return;
            }

            Move(direction: CalculateDirection());
        }

        public void Initialize(InputService inputService)
        {
            this.inputService = inputService;
            isInitialized = true;
        }

        private Vector3 CalculateDirection()
        {
            var xInput = 0;
            var zInput = 0;

            if (inputService.UpPressed)
                zInput += 1;
            if (inputService.DownPressed)
                zInput -= 1;
            if (inputService.LeftPressed)
                xInput -= 1;
            if (inputService.RightPressed)
                xInput += 1;

            if (xInput != 0 || zInput != 0)
            {
                characterAnimationController.SetWalking(true);
            }
            else
            {
                characterAnimationController.SetWalking(false);
            }

            return new Vector3(xInput, 0, zInput).normalized;
        }

    }
}