using System.Linq;
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

        [SerializeField] private UnitWeapon weapon;

        public IUnitStatusProvider StatusProvider => status;
        public TargetSelectorFactory TargetSelectorFactory => targetSelectorFactory;

        private void Update()
        {
            state?.Process(Time.deltaTime);
        }

        public void Initialize(UnitFaction faction, TargetSelectorFactory targetSelectorFactory)
        {
            status = new UnitStatusFactory().Create(faction, Configuration, weapon);

            weapon.Initialize(this, Configuration.BaseDamage.Select(damageConfig => damageConfig.DamageType));

            this.targetSelectorFactory = targetSelectorFactory;
            ComponentsContainer.InitializeComponents(status);
            var stateData = new UnitStateData
            (
                ownerComponents: ComponentsContainer,
                ownerStatus: status,
                ownerController: this
            );

            stateFactory = new UnitStateFactory(stateData);
        }

        public void SetState(UnitState state)
        {
            this.state?.Exit();

            this.state = stateFactory.CreateState(state);
            status.State = state;

            this.state?.Enter();
        }
    }
}