using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterMovementController : MonoBehaviour, IInitializableCharacterComponent
    {
        public event Action DashStarted;
        public event Action DashCompleted;
        public event Action DashCooldownCompleted;

        [SerializeField] protected float movementSpeed;
        [SerializeField] protected float rotationDuration = 0.2f;
        [SerializeField] protected CharacterController characterController;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCooldown;

        private float passedDashTime;
        private float passedDashCooldownTime;

        public float DashCooldownDuration => dashCooldown;
        public float PassedDashCooldownTime => passedDashCooldownTime;
        public float MovementSpeed => movementSpeed;

        private Transform characterTransform;

        protected ICharacterStateProvider CharacterState { get; private set; }
        protected ServiceLocator ServiceLocator { get; private set; }

        public virtual void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state)
        {
            ServiceLocator = serviceLocator;
            CharacterState = state;

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

            characterController.Move(direction * movementSpeed * Time.fixedDeltaTime);
            var rotation = Quaternion.LookRotation(direction);
            characterTransform.DORotateQuaternion(rotation, rotationDuration);
        }

        public void PerformDash()
        {
            if (CharacterState.IsDead /**|| CharacterState.IsMovementLocked**/)
            {
                return;
            }

            StartCoroutine(Dash());
            StartCoroutine(DashCooldown());

            DashStarted?.Invoke();
        }

        private IEnumerator Dash()
        {
            var passedDashTime = 0f;

            while (passedDashTime < dashDuration)
            {
                passedDashTime += Time.fixedDeltaTime;
                characterController
                    .Move(dashSpeed * Time.fixedDeltaTime * ServiceLocator.InputService.InputDirection);

                yield return new WaitForFixedUpdate();
            }

            DashCompleted?.Invoke();
        }

        private IEnumerator DashCooldown()
        {
            passedDashCooldownTime = 0f;

            while (passedDashCooldownTime < dashCooldown)
            {
                passedDashCooldownTime += Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }

            DashCooldownCompleted?.Invoke();
        }
    }
}