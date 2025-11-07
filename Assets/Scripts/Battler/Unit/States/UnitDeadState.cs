using UnityEngine;

namespace AutoBattler
{
    public class UnitDeadState : UnitStateBase
    {
        private UnitAnimationController animationController;

        public UnitDeadState(UnitStateData stateData) : base(stateData)
        {
            animationController = stateData.OwnerComponents.AnimationController;
        }

        public override void Enter()
        {
            base.Enter();

            animationController.SetDeath(true);
            SetPhysicsLayer(isDeadLayer: true);
        }

        public override void Exit()
        {
            base.Exit();

            SetPhysicsLayer(isDeadLayer: false);
            animationController.SetDeath(false);
        }

        private void SetPhysicsLayer(bool isDeadLayer)
        {
            var layer = isDeadLayer ? PhysicsLayersUtility.UnitDeadLayer : PhysicsLayersUtility.UnitLayer;
            var gameObject = StateData.OwnerController.gameObject;

            foreach (var children in gameObject.GetComponentsInChildren<Collider>())
            {
                children.gameObject.layer = LayerMask.NameToLayer(layer);
            }

            gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }
}