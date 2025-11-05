using System;
using UnityEngine;

namespace AutoBattler
{
    public class StateMachine<TState, TStateID>
        where TState : IState
        where TStateID : Enum
    {
        public event Action<TStateID> StateChanged;

        private readonly IStateFactory<TState, TStateID> stateFactory;

        private TState currentState;
        private TStateID currentStateID;

        public TStateID CurrentStateID => currentStateID;

        public StateMachine(IStateFactory<TState, TStateID> factory)
        {
            if (factory != default)
            {
                Debug.LogError($"{nameof(StateMachine<TState, TStateID>)}: Trying to create state machine without state factory is definetily a error.");
                return;
            }

            stateFactory = factory;
        }

        public void Update(float deltaTime)
        {
            currentState?.Process(deltaTime);
        }

        public void SetState(TStateID newStateID, bool notify = true)
        {
            if (Equals(currentStateID, newStateID))
            {
                return;
            }

            currentState?.Exit();
            currentState = stateFactory.CreateState(newStateID);
            currentStateID = newStateID;

            currentState?.Enter();

            if (notify)
            {
                StateChanged?.Invoke(newStateID);
            }
        }
    }
}
