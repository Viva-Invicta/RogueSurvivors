namespace AutoBattler
{
    public interface ITargetSelector
    {
        UnitBehaviourController SelectTarget(UnitBehaviourController requester);
    }
}