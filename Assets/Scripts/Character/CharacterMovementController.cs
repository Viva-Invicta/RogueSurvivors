using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterMovementController : MonoBehaviour, IInitializableCharacterComponent
    {
        public event Action MovementSpeedUpdated;

        public event Action DashStarted;
        public event Action DashCompleted;
        public event Action DashCooldownCompleted;

        [SerializeField] protected float initialMovementSpeed;
        [SerializeField] protected float rotationDuration = 0.2f;
        [SerializeField] protected CharacterController characterController;
        [SerializeField] private float initialDashSpeed;
        [SerializeField] private float initialDashDuration;
        [SerializeField] private float initialDashCooldown;

        private float passedDashCooldownTime;

        public float PassedDashCooldownTime => passedDashCooldownTime;

        public float DashCooldownDuration => initialDashCooldown * dashCooldownModifier;
        public float MovementSpeed => initialMovementSpeed * movementSpeedModifier;
        private float DashSpeed => initialDashSpeed * dashSpeedModifier;
        private float DashDuration => initialDashDuration * dashDurationModifier;

        private Transform characterTransform;

        private Modifier movementSpeedModifier;
        private Modifier dashSpeedModifier;
        private Modifier dashDurationModifier;
        private Modifier dashCooldownModifier;

        private Tween rotationTween;

        protected ICharacterStateProvider CharacterState { get; private set; }
        protected ServiceLocator ServiceLocator { get; private set; }

        public virtual void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
            ServiceLocator = serviceLocator;
            CharacterState = state;

            var modifiersService = serviceLocator.ModifiersService;

            var faction = state.Faction;

            movementSpeedModifier = modifiersService.GetModifier(faction, ModifierType.MovementSpeed);
            dashDurationModifier = modifiersService.GetModifier(faction, ModifierType.DashDuration);
            dashCooldownModifier = modifiersService.GetModifier(faction, ModifierType.DashCooldown);
            dashSpeedModifier = modifiersService.GetModifier(faction, ModifierType.DashSpeed);

            movementSpeedModifier.Updated += HandleMovementSpeedModifierUpdated;

            characterTransform = characterController.transform;
        }

        public void MoveToPoint(Vector3 point)
        {
            Move((point - transform.position).normalized);
        }

        public void Move(Vector3 direction)
        {
            if (CharacterState.IsDead || CharacterState.IsMovementLocked)
            {
                return;
            }

            if (direction == Vector3.zero)
            {
                return;
            }

            characterController.Move(direction * MovementSpeed * Time.fixedDeltaTime);
            var rotation = Quaternion.LookRotation(direction);
            rotationTween?.Kill();
            rotationTween = characterTransform.DORotateQuaternion(rotation, rotationDuration);
        }

        public void PerformDash()
        {
            if (CharacterState.IsDead)
            {
                return;
            }

            StartCoroutine(Dash());
            StartCoroutine(DashCooldown());

            DashStarted?.Invoke();
        }

        private void HandleMovementSpeedModifierUpdated()
        {
            MovementSpeedUpdated?.Invoke();
        }

        private IEnumerator Dash()
        {
            var passedDashTime = 0f;

            while (passedDashTime < DashDuration)
            {
                passedDashTime += Time.fixedDeltaTime;
                characterController
                    .Move(DashSpeed * Time.fixedDeltaTime * ServiceLocator.InputService.InputDirection);

                yield return new WaitForFixedUpdate();
            }

            DashCompleted?.Invoke();
        }

        private IEnumerator DashCooldown()
        {
            passedDashCooldownTime = 0f;

            while (passedDashCooldownTime < DashCooldownDuration)
            {
                passedDashCooldownTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            DashCooldownCompleted?.Invoke();
        }
    }
}