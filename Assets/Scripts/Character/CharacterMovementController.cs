using DG.Tweening;
using UnityEngine;

namespace DunDungeons
{
    public class CharacterMovementController : MonoBehaviour
    {
        [SerializeField] protected float movementSpeed;
        [SerializeField] protected float rotationDuration = 0.2f;

        protected CharacterController characterController;
        private CharacterBehaviourController characterBehaviourController;
        protected bool isMovementLocked;

        public bool IsLocked => isMovementLocked;

        protected virtual void OnEnable()
        {
            characterController = GetComponent<CharacterController>();
            characterBehaviourController = GetComponent<CharacterBehaviourController>();

            Debug.Assert(characterController, "No Character Controller!");
        }

        public void LockMovement(bool isLocked)
        {
            isMovementLocked = isLocked;
        }

        public void MoveToPoint(Vector3 point)
        {
            Move((point - transform.position).normalized);
        }

        protected void Move(Vector3 direction)
        {
            if (characterBehaviourController.IsDead)
            {
                return;
            }

            if (isMovementLocked)
            {
                return;
            }

            if (direction == Vector3.zero)
            {
                return;
            }

            characterController.Move(direction * movementSpeed * Time.fixedDeltaTime);
            var rotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(rotation, rotationDuration);
        }
    }
}