namespace AutoBattler
{
    public abstract class UnitStateBase : IState
    {
        protected UnitStateData StateData;

        public UnitStateBase(UnitStateData stateData)
        {
            StateData = stateData;
        }

        public virtual void Enter() { }
        public virtual void Process(float deltaTime) { }
        public virtual void Exit() { }
    }

    public class UnitStateData
    {
        public UnitComponentsContainer OwnerComponents { get; private set; }
        public UnitBehaviourController OwnerController { get; private set; }
        public UnitStatus OwnerStatus { get; private set; }

        public UnitStateData(UnitComponentsContainer ownerComponents, UnitBehaviourController ownerController, UnitStatus ownerStatus)
        {
            OwnerComponents = ownerComponents;
            OwnerController = ownerController;
            OwnerStatus = ownerStatus;
        }

    }
}