using UnityEngine;

namespace AutoBattler
{
    public class UnitPreviewState : UnitStateBase
    {
        public UnitPreviewState(UnitBehaviourController stateOwner) : base(stateOwner)
        {
        }

        public override void Enter()
        {
            base.Enter();

            SetPreviewAnimation(isPreview: true);
            SetPhysicsLayer(isPreviewLayer: true);
        }

        public override void Exit()
        {
            base.Exit();

            SetPreviewAnimation(isPreview: false);
            SetPhysicsLayer(isPreviewLayer: false);
        }

        private void SetPreviewAnimation(bool isPreview)
        {
            if (StateOwner)
            {
                var animationController = StateOwner.ComponentsContainer.AnimationController;
                animationController.SetPreview(isPreview);
            }
        }

        private void SetPhysicsLayer(bool isPreviewLayer)
        {
            var layer = isPreviewLayer ? PhysicsLayersUtility.UnitPreviewLayer : PhysicsLayersUtility.UnitLayer;
            var gameObject = StateOwner.gameObject;

            foreach (var children in gameObject.GetComponentsInChildren<Collider>())
            {
                children.gameObject.layer = LayerMask.NameToLayer(layer);
            }

            gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }
}