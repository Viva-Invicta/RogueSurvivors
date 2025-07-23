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

        private EntitiesService entitiesService;

        private HealthComponent playerHealth;
        private bool canWalk = false;

        protected override void OnAfterInitialize()
        {
            entitiesService = serviceLocator.EntitiesService;

            var player = entitiesService.Player;
            if (player)
            {
                playerHealth = player.GetComponent<HealthComponent>();
                playerHealth.Updated += HandlePlayerHealthUpdated;
            }
            StartCoroutine(DelayBeforeCanWalk());
        }

        private void HandlePlayerHealthUpdated()
        {
            if (playerHealth.CurrentHP <= 0)
            {
                State.IsAttackLocked = true;
                State.IsMovementLocked = true;
                State.IsDead = true;
                animationController.PlayCheer();
            }
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

            if ((Mathf.Abs(distanceToPlayer) > distanceToAttack && !State.IsMovementLocked))
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

        protected override void OnAfterDestroy()
        {
            playerHealth.Updated -= HandlePlayerHealthUpdated;
        }
    }
}