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
            animationController.PlayDeath();
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}