using UnityEngine;

namespace AutoBattler
{
    public class UnitPreviewState : UnitStateBase
    {
        public UnitPreviewState(UnitStateData stateData) : base(stateData)
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
            var animationController = StateData.OwnerComponents.AnimationController;
            animationController.SetPreview(isPreview);
        }

        private void SetPhysicsLayer(bool isPreviewLayer)
        {
            var layer = isPreviewLayer ? PhysicsLayersUtility.UnitPreviewLayer : PhysicsLayersUtility.UnitLayer;
            var gameObject = StateData.OwnerController.gameObject;

            foreach (var children in gameObject.GetComponentsInChildren<Collider>())
            {
                children.gameObject.layer = LayerMask.NameToLayer(layer);
            }

            gameObject.layer = LayerMask.NameToLayer(layer);
        }
    }
}