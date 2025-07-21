using DG.Tweening;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterMovementController : MonoBehaviour, IInitializableCharacterComponent
    {
        [SerializeField] protected float movementSpeed;
        [SerializeField] protected float rotationDuration = 0.2f;
        [SerializeField] protected CharacterController characterController;

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
    }
}