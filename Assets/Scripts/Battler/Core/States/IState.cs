using System;

namespace AutoBattler
{
    public interface IState<TStateID>
        where TStateID : Enum
    {
        public event Action<TStateID> StateChangeRequest;
        public void Enter();
        public void Exit();
        public void Process(float deltaTime);
    }
}