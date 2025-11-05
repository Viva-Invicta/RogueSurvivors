using System;

namespace AutoBattler
{
    public abstract class GameStateBase : IState<GameStateID>
    {
        public virtual event Action<GameStateID> StateChangeRequest;

        protected ServiceLocator ServiceLocator;

        public GameStateBase(ServiceLocator serviceLocator)
        {
            ServiceLocator = serviceLocator;
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void Process(float deltaTime)
        {
        }
    }
}