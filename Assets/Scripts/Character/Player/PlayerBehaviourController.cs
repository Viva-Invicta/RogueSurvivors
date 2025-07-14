using UnityEngine;

namespace DunDungeons
{
    public class PlayerBehaviourController : CharacterBehaviourController
    {
        [SerializeField]
        private PlayerCombatController combatController;

        [SerializeField]
        private PlayerMovementController movementController;

        public override void Initialize(ServiceLocator serviceLocator)
        {
            combatController.Initialize(serviceLocator.InputService);
            movementController.Initialize(serviceLocator.InputService);
        }
    }
}