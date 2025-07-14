using System.Collections;
using UnityEngine;

namespace DunDungeons
{
    public class SkeletonBehaviourController : CharacterBehaviourController
    {
        [SerializeField]
        private float distanceToAttack = 0.5f;

        [SerializeField]
        private float minDelayBeforeWalk = 0f;

        [SerializeField]
        private float maxDelayBeforeWalk = 5f;

        private CharacterCombatController combatController;
        private CharacterMovementController movementController;
        private CharacterAnimationController animationController;

        private EntitiesService entitiesService;

        private bool canWalk = false;

        protected override void OnEnable()
        {
            base.OnEnable();

            combatController = GetComponent<CharacterCombatController>();
            movementController = GetComponent<CharacterMovementController>();
            animationController = GetComponent<CharacterAnimationController>();

            if (!combatController)
            {
                Debug.LogError("No combat controller on " + gameObject.name);
            }

            if (!movementController)
            {
                Debug.LogError("No movement controller on " + gameObject.name);
            }

            if (!animationController)
            {
                Debug.LogError("No animation controller on " + gameObject.name);
            }

        }

        public override void Initialize(ServiceLocator serviceLocator)
        {
            entitiesService = serviceLocator.EntitiesService;

            StartCoroutine(DelayBeforeCanWalk());
        }

        private IEnumerator DelayBeforeCanWalk()
        {
            var delayBeforeWalk = Random.Range(minDelayBeforeWalk, maxDelayBeforeWalk);
            yield return new WaitForSecondsRealtime(delayBeforeWalk);
            canWalk = true;
        }

        private void FixedUpdate()
        {
            var player = entitiesService.Player;
            if (!player)
            {
                return;
            }

            if (!canWalk)
            {
                return;
            }

            var playerPosition = player.transform.position;
            var distanceToPlayer = (transform.position - playerPosition).magnitude;

            if (Mathf.Abs(distanceToPlayer) > distanceToAttack && !movementController.IsLocked)
            {
                animationController.SetWalking(true);
                movementController.MoveToPoint(player.transform.position);
            }
            else
            {
                animationController.SetWalking(false);
                combatController.Attack();
            }
        }
    }
}