using UnityEngine;

namespace AutoBattler
{
    public class UnitBehaviourController : MonoBehaviour
    {
        private UnitStateBase state;
        private UnitStatus status;
        private IUnitStateFactory stateFactory;
        private TargetSelectorFactory targetSelectorFactory;

        [field: SerializeField] public UnitConfiguration Configuration { get; private set; }
        [field: SerializeField] public UnitComponentsContainer ComponentsContainer { get; private set; }

        public IUnitStatusProvider StatusProvider => status;
        public TargetSelectorFactory TargetSelectorFactory => targetSelectorFactory;

        private void Update()
        {
            state?.Process(Time.deltaTime);
        }

        public void Initialize(UnitFaction faction, TargetSelectorFactory targetSelectorFactory)
        {
            stateFactory = new UnitStateFactory();
            status = new UnitStatusFactory().Create(faction, Configuration);
            this.targetSelectorFactory = targetSelectorFactory;

            ComponentsContainer.InitializeComponents(status);
        }

        public void SetState(UnitState state)
        {
            this.state?.Exit();

            this.state = stateFactory.CreateState(state, this);
            status.State = state;

            this.state?.Enter();
        }

        public void RecieveDamage(DamageType damageType, float value)
        {
            var damage = status.UnitValuesCalculator.CalculateIncomingDamage(value, damageType);
            status.Health.Consume(damage);
        }
    }
}