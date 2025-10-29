using UnityEngine;

namespace AutoBattler
{
    public class UnitPreviewState : UnitStateBase
    {
        public UnitPreviewState(UnitBehaviourController stateOwner) : base(stateOwner)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            SetPreviewAnimation(isPreview: true);
            SetPhysicsLayer(isPreviewLayer: true);
        }

        public override void ExitState()
        {
            base.ExitState();

            SetPreviewAnimation(isPreview: false);
            SetPhysicsLayer(isPreviewLayer: false);
        }

        private void SetPreviewAnimation(bool isPreview)
        {
            if (StateOwner.AnimationController)
            {
                StateOwner.AnimationController.SetPreview(isPreview);
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