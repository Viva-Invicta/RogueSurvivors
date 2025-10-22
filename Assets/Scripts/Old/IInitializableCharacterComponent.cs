namespace DunDungeons
{
    public interface IInitializableCharacterComponent
    {
        public void Initialize(ServiceLocator serviceLocator, ICharacterStateProvider state);
    }
}