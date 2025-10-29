using UnityEngine;

namespace AutoBattler
{
    public class UnitFightingState : UnitStateBase
    {
        //todo move!!!
        private const float UpdateTargetEveryNSeconds = 1f;

        private UnitBehaviourController activeTarget;

        private ITargetSelector targetSelector;
        private Transform ownerTransform;
        private UnitCombatController combatController;
        private UnitMovementController movementController;
        private UnitAnimationController animationController;
        private float distanceToAttack;

        private float timeSinceLastTargetUpdate = 0f;

        public UnitFightingState(UnitBehaviourController stateOwner) : base(stateOwner)
        {
        }

        public override void Enter()
        {
            base.Enter();

            targetSelector = StateOwner.TargetSelectorFactory.GetSelector(TargetSelectorType.NearestEnemy);

            ownerTransform = StateOwner.transform;

            var componentsContainer = StateOwner.ComponentsContainer;

            combatController = componentsContainer.CombatController;
            movementController = componentsContainer.MovementController;
            animationController = componentsContainer.AnimationController;

            distanceToAttack = StateOwner.Configuration.BaseAttackDistance;

            UpdateTarget();
        }

        public override void Process(float deltaTime)
        {
            base.Process(deltaTime);

            timeSinceLastTargetUpdate += deltaTime;
            if (timeSinceLastTargetUpdate > UpdateTargetEveryNSeconds)
            {
                UpdateTarget();
            }

            if (activeTarget)
            {
                var targetPosition = activeTarget.transform.position;
                var distanceToTarget = Vector3.Distance(ownerTransform.position, targetPosition);
                if (distanceToTarget <= distanceToAttack)
                {
                    combatController.Attack(activeTarget);
                    animationController.TriggerAttackAnimation();
                }
                else
                {
                    animationController.SetWalking(true);
                    movementController.MoveToPoint(targetPosition);
                }
            }
        }

        public override void Exit()
        {
            animationController.SetWalking(false);
            base.Exit();
        }

        private void UpdateTarget()
        {
            activeTarget = targetSelector.SelectTarget(requester: StateOwner);
        }
    }
}