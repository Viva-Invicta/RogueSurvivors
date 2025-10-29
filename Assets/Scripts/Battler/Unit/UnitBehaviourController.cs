using System.Collections.Generic;
using UnityEngine;

namespace AutoBattler
{
    public class UnitBehaviourController : MonoBehaviour
    {
        private UnitStateBase unitState;
        private UnitStatus unitStatus;
        private IUnitStateFactory stateFactory;

        [field: SerializeField] public UnitConfiguration Configuration { get; private set; }
        [field: SerializeField] public UnitComponentsContainer ComponentsContainer { get; private set; }

        public IUnitStatusProvider UnitStatusProvider => unitStatus;

        public void Initialize(UnitFaction faction)
        {
            stateFactory = new UnitStateFactory();
            unitStatus = new UnitStatusFactory().Create(faction, Configuration);

            ComponentsContainer.InitializeComponents(unitStatus);
        }

        public void SetState(UnitState state)
        {
            unitState?.ExitState();

            unitState = stateFactory.CreateState(state, this);
            unitStatus.State = state;

            unitState?.EnterState();
        }

        public void RecieveDamage(DamageType damageType, float value)
        {
            var damage = unitStatus.UnitValuesCalculator.CalculateIncomingDamage(value, damageType);
            unitStatus.Health.Consume(damage);
        }
    }
}