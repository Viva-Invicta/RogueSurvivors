using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace AutoBattler
{
    public class UnitMovementController : MonoBehaviour, IInitializableWithUnitStatusComponent
    {
        public event Action MovementSpeedUpdated;

        public event Action DashStarted;
        public event Action DashCompleted;
        public event Action DashCooldownCompleted;

        [SerializeField] private float rotationDuration = 0.2f;
        [SerializeField] private CharacterController characterController;

        private Transform characterTransform;
        private Tween rotationTween;

        private IUnitStatusProvider unitStatus;

        protected ServiceLocator ServiceLocator { get; private set; }

        public virtual void Initialize(IUnitStatusProvider unitStatus)
        {
            this.unitStatus = unitStatus;
            characterTransform = characterController.transform;
        }

        public void MoveToPoint(Vector3 point)
        {
            Move((point - transform.position).normalized);
        }

        public void Move(Vector3 direction)
        {
            if (unitStatus.State != UnitState.Fight || unitStatus.IsMovementLocked)
            {
                return;
            }

            if (direction == Vector3.zero)
            {
                return;
            }

            characterController.Move(direction * unitStatus.BaseMovementSpeed * Time.fixedDeltaTime);

            var rotation = Quaternion.LookRotation(direction);
            rotationTween?.Kill();
            rotationTween = characterTransform.DORotateQuaternion(rotation, rotationDuration);
        }
    }
}