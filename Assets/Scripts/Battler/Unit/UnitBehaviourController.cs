using UnityEngine;

namespace AutoBattler
{ 
    public class UnitBehaviourController : MonoBehaviour
    {
        private UnitStateBase unitState;
        private UnitStatus unitStatus;
        private UnitState currentState;
        private IUnitStateFactory stateFactory;

        [field: SerializeField] public UnitConfiguration Configuration { get; private set; }
        [field: SerializeField] public UnitAnimationController AnimationController { get; private set; }

        public UnitState State => currentState;
        public IUnitStatusProvider UnitStatusProvider => unitStatus;

        public void Initialize(UnitFaction faction, IUnitStateFactory factory = null)
        {
            unitStatus = new UnitStatus(faction);
            stateFactory = factory ?? new UnitStateFactory();
        }

        public void SetState(UnitState state)
        {
            unitState?.ExitState();
            currentState = state;
            unitState = stateFactory.CreateState(state, this);
            unitState?.EnterState();
        }
    }
}