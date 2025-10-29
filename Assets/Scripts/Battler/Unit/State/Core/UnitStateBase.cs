namespace AutoBattler
{
    public abstract class UnitStateBase
    {
        protected UnitBehaviourController StateOwner;

        public UnitStateBase(UnitBehaviourController stateOwner)
        {
            StateOwner = stateOwner;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }

        public virtual void Process(float deltaTime) { }
    }
}