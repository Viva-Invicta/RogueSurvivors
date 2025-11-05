namespace AutoBattler
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void Process(float deltaTime);
    }
}