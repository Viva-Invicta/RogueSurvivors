using System;
using UnityEngine;

namespace AutoBattler
{
    public class UnitFightingState : UnitStateBase
    {
        public override event Action<UnitStateID> StateChangeRequest;

        private UnitBehaviourController activeTarget;
        private ITargetSelector targetSelector;
        private Transform ownerTransform;
        private UnitCombatControllerBase combatController;
        private UnitMovementController movementController;
        private UnitAnimationController animationController;
        private Resource health;

        private float distanceToAttack;
        private float timeSinceLastTargetUpdate = 0f;
        private float timeToUpdateTarget;

        public UnitFightingState(UnitStateData stateData) : base(stateData)
        {
            ValidateStateData(stateData);
        }

        public override void Enter()
        {
            base.Enter();
            InitializeState();
        }

        public override void Process(float deltaTime)
        {
            base.Process(deltaTime);
            ProcessCombat(deltaTime);
        }

        public override void Exit()
        {
            Cleanup();
            base.Exit();
        }

        private void InitializeState()
        {
            if (!ValidateStateData())
            {
                return;
            }

            GetRequiredComponents();
            InitializeCombatParameters();
            SubscribeToEvents();
            UpdateTarget();
        }

        private void ProcessCombat(float deltaTime)
        {
            timeSinceLastTargetUpdate += deltaTime;
            if (timeSinceLastTargetUpdate > timeToUpdateTarget)
            {
                UpdateTarget();
            }

            ProcessTargetChase();
        }

        private void ProcessTargetChase()
        {
            if (!activeTarget)
            {
                animationController.SetWalking(false);
                return;
            }

            var targetPosition = activeTarget.transform.position;
            var distanceToTarget = Vector3.Distance(ownerTransform.position, targetPosition);

            var ownerStatus = StateData.OwnerStatus;

            if (distanceToTarget <= distanceToAttack)
            {
                animationController.SetWalking(false); // стоим на месте
                if (!ownerStatus.IsAttackInCooldown && !ownerStatus.IsAttackLocked)
                {
                    ExecuteAttack();
                }
            }
            else
            {
                ChaseTarget(targetPosition);
            }
        }

        private bool ValidateStateData()
        {
            if (StateData == default)
            {
                Debug.LogError($"{nameof(UnitFightingState)} : StateData is null in Enter");
                return false;
            }

            if (StateData.OwnerStatus.Configuration == default)
            {
                Debug.LogError($"{nameof(UnitFightingState)} : OwnerConfiguration is null");
                return false;
            }

            return true;
        }

        private void GetRequiredComponents()
        {
            var stateOwner = StateData.OwnerController;
            var componentsContainer = StateData.OwnerComponents;

            GetTargetSelector(stateOwner);
            ownerTransform = stateOwner.transform;
            GetCombatComponents(componentsContainer);
        }

        private void GetTargetSelector(UnitBehaviourController stateOwner)
        {
            targetSelector = stateOwner.TargetSelectorFactory.GetSelector(StateData.OwnerStatus.Configuration.TargetSelector);

            if (targetSelector == default)
            {
                Debug.LogError($"{nameof(UnitFightingState)} : TargetSelector is null");
            }
        }

        private void GetCombatComponents(UnitComponentsContainer componentsContainer)
        {
            combatController = GetAndValidateComponent(componentsContainer.CombatController, nameof(combatController));
            movementController = GetAndValidateComponent(componentsContainer.MovementController, nameof(movementController));
            animationController = GetAndValidateComponent(componentsContainer.AnimationController, nameof(animationController));
        }

        private T GetAndValidateComponent<T>(T component, string componentName) where T : UnityEngine.Object
        {
            if (!component)
            {
                Debug.LogError($"{nameof(UnitFightingState)} : {componentName} is null");
            }
            return component;
        }

        private void InitializeCombatParameters()
        {
            timeToUpdateTarget = StateData.OwnerStatus.Configuration.TimeToUpdateTarget;
            distanceToAttack = StateData.OwnerController.Configuration.BaseAttackDistance;
            health = StateData.OwnerStatus.Health;

            ValidateCombatParameters();
        }

        private void ValidateCombatParameters()
        {
            if (distanceToAttack <= 0f)
            {
                Debug.LogWarning($"{nameof(UnitFightingState)} : DistanceToAttack is invalid: {distanceToAttack}");
            }

            if (health == default)
            {
                Debug.LogError($"{nameof(UnitFightingState)} : Health resource is null");
            }
        }

        private void SubscribeToEvents()
        {
            if (combatController)
            {
                combatController.StartedCooldown += HandleAttackCooldownStarted;
                combatController.EndedCooldown += HandleAttackCooldownEnded;
            }

            if (health != default)
            {
                health.Updated += HandleHealthUpdated;
            }
        }

        private void ExecuteAttack()
        {
            combatController.Attack(activeTarget);
            animationController.TriggerAttackAnimation();
        }

        private void ChaseTarget(Vector3 targetPosition)
        {
            animationController.SetWalking(true);
            movementController.MoveToPoint(targetPosition);
        }

        private void Cleanup()
        {
            if (animationController)
            {
                animationController.SetWalking(false);
            }


            UnsubscribeFromEvents();

            var ownerStatus = StateData.OwnerStatus;
            ownerStatus.IsAttackInCooldown = false;
            ownerStatus.IsMovementLocked = false;

            ClearReferences();
        }

        private void UnsubscribeFromEvents()
        {
            if (combatController)
            {
                combatController.StartedCooldown -= HandleAttackCooldownStarted;
                combatController.EndedCooldown -= HandleAttackCooldownEnded;
            }

            if (health != default)
            {
                health.Updated -= HandleHealthUpdated;
            }
        }

        private void ClearReferences()
        {
            targetSelector = default;
            ownerTransform = default;
            combatController = default;
            movementController = default;
            animationController = default;
            activeTarget = default;
        }

        private void UpdateTarget()
        {
            if (targetSelector == default)
            {
                Debug.LogError($"{nameof(UnitFightingState)} : TargetSelector is null");
                return;
            }

            activeTarget = targetSelector.SelectTarget(requester: StateData.OwnerController);
            timeSinceLastTargetUpdate = 0f;
        }

        private void HandleHealthUpdated()
        {
            if (health.CurrentValue <= 0)
            {
                StateChangeRequest?.Invoke(UnitStateID.Dead);
            }
        }

        private void HandleAttackCooldownStarted(float cooldown)
        {
            if (cooldown <= 0f)
            {
                Debug.LogWarning($"{nameof(UnitFightingState)} : Invalid cooldown value: {cooldown}");
                return;
            }

            animationController.SetAttackSpeed(1 / cooldown);

            var ownerStatus = StateData.OwnerStatus;
            ownerStatus.IsAttackInCooldown = true;
            ownerStatus.IsMovementLocked = true;
        }

        private void HandleAttackCooldownEnded()
        {
            var ownerStatus = StateData?.OwnerStatus;
            if (ownerStatus != null)
            {
                ownerStatus.IsAttackInCooldown = false;
                ownerStatus.IsMovementLocked = false;
            }
        }

        private void ValidateStateData(UnitStateData stateData)
        {
            if (stateData == default)
            {
                Debug.LogError($"{nameof(UnitFightingState)} : StateData is null");
            }
        }
    }
}