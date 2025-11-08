namespace AutoBattler
{
    public class UnitWaitingState : UnitStateBase
    {
        private UnitEffectsController effectsController;

        public UnitWaitingState(UnitStateData stateData) : base(stateData)
        {
            effectsController = stateData.OwnerComponents.EffectsController;
        }

        public override void Enter()
        {
            base.Enter();

            effectsController.PlayEffect(UnitEffectType.Spawn);
        }

        public override void Exit()
        {
            base.Exit();

            effectsController = null;
        }
    }
}