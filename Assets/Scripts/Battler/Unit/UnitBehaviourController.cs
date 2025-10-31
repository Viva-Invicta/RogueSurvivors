using System;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class UnitBehaviourController : MonoBehaviour, IEntityWithGridPosition
    {
        public event Action StateUpdated;

        private UnitStateBase state;
        private UnitStatus status;
        private IUnitStateFactory stateFactory;
        private TargetSelectorFactory targetSelectorFactory;

        [field: SerializeField] public UnitConfiguration Configuration { get; private set; }
        [field: SerializeField] public UnitComponentsContainer ComponentsContainer { get; private set; }

        [SerializeField] private UnitWeapon weapon;

        public IUnitStatusProvider StatusProvider => status;
        public TargetSelectorFactory TargetSelectorFactory => targetSelectorFactory;

        public (int x, int y) GridPosition => status.GridPosition;

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

        public void SetState(UnitState state, bool notificate = true)
        {
            this.state?.Exit();

            this.state = stateFactory.CreateState(state);
            status.State = state;

            this.state?.Enter();

            if (notificate)
            {
                StateUpdated?.Invoke();
            }
        }

        public void SetGridPosition(int x, int y)
        {
            status.GridPosition = (x, y);
        }
    }
}