using UnityEngine;

namespace AutoBattler
{
    public class UnitDeadState : UnitStateBase
    {
        private UnitAnimationController animationController;
        private UnitEffectsController effectsController;

        public UnitDeadState(UnitStateData stateData) : base(stateData)
        {
            var ownerComponents = stateData.OwnerComponents;

            animationController = ownerComponents.AnimationController;
            effectsController = ownerComponents.EffectsController;
        }

        public override void Enter()
        {
            base.Enter();

            animationController.SetDeath(true);
            SetPhysicsLayer(isDeadLayer: true);

            effectsController.PlayEffect(UnitEffectType.Death);
        }

        public override void Exit()
        {
            base.Exit();

            SetPhysicsLayer(isDeadLayer: false);
            animationController.SetDeath(false);

            effectsController = default;
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