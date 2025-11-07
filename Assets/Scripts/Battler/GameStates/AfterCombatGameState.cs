using System;

namespace AutoBattler
{
    public class AfterCombatGameState : GameStateBase
    {
        //it is some boilerplate for showing lose/win screen later.

        public override event Action<GameStateID> StateChangeRequest;

        private const float delayDuration = 3f;
        private float passedTime;

        public AfterCombatGameState(ServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override void Process(float deltaTime)
        {
            base.Process(deltaTime);

            passedTime += deltaTime;

            if (passedTime > delayDuration)
            {
                StateChangeRequest?.Invoke(GameStateID.CombatCleanup);
            }
        }
    }
}