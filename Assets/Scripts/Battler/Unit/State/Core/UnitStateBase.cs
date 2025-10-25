namespace AutoBattler
{
    public abstract class UnitStateBase
    {
        protected UnitBehaviourController StateOwner;

        public UnitStateBase(UnitBehaviourController stateOwner)
        {
            StateOwner = stateOwner;
        }

        public virtual void EnterState() { }
        public virtual void ExitState() { }
    }
}