namespace AutoBattler
{
    public interface IUnitStateFactory
    {
        UnitStateBase CreateState(UnitState state, UnitBehaviourController controller);
    }
}