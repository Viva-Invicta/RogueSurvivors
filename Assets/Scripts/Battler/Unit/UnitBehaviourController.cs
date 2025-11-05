using System;
using System.Linq;
using UnityEngine;

namespace AutoBattler
{
    public class UnitBehaviourController : MonoBehaviour, IEntityWithGridPosition
    {
        public event Action StateUpdated;

        private StateMachine<UnitStateBase, UnitStateID> stateMachine;
        private IStateFactory<UnitStateBase, UnitStateID> stateFactory;

        private UnitStatus status;
        private TargetSelectorFactory targetSelectorFactory;

        [field: SerializeField] public UnitConfiguration Configuration { get; private set; }
        [field: SerializeField] public UnitComponentsContainer ComponentsContainer { get; private set; }

        [SerializeField] private UnitWeapon weapon;

        public StateMachine<UnitStateBase, UnitStateID> StateMachine => stateMachine;
        public IUnitStatusProvider StatusProvider => status;
        public TargetSelectorFactory TargetSelectorFactory => targetSelectorFactory;

        public (int x, int y) GridPosition => status.GridPosition;

        private Action<UnitStateID> onStateChangedHandler;

        private void Update()
        {
            stateMachine?.Update(Time.deltaTime);
        }

        public void Initialize(UnitFaction faction, TargetSelectorFactory targetSelectorFactory)
        {
            status = new UnitStatusFactory().Create(faction, Configuration, weapon);
            weapon.Initialize(this, Configuration.BaseDamage.Select(damageEntry => damageEntry.DamageType));

            this.targetSelectorFactory = targetSelectorFactory;
            ComponentsContainer.InitializeComponents(status);

            var stateData = new UnitStateData(
                ownerComponents: ComponentsContainer,
                ownerStatus: status,
                ownerController: this
            );

            stateFactory = new UnitStateFactory(stateData);
            stateMachine = new StateMachine<UnitStateBase, UnitStateID>(stateFactory);

            onStateChangedHandler = _ => StateUpdated?.Invoke();
            stateMachine.StateChanged += onStateChangedHandler;
        }

        public void SetGridPosition(int x, int y)
        {
            status.GridPosition = (x, y);
        }

        private void OnDestroy()
        {
            if (stateMachine != null && onStateChangedHandler != null)
            {
                stateMachine.StateChanged -= onStateChangedHandler;
                onStateChangedHandler = null;
            }

            StateUpdated = null;
            stateMachine = null;
            stateFactory = null;
            status = null;
            targetSelectorFactory = null;
        }
    }
}
