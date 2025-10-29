namespace AutoBattler
{
    public interface IInitializableWithUnitStatusComponent
    {
        public void Initialize(IUnitStatusProvider unitStatusProvider);
    }
}