using System;
using UnityEngine;

namespace AutoBattler
{
    public class StateMachine<TState, TStateID>
        where TState : IState<TStateID>
        where TStateID : Enum
    {
        public event Action<TStateID> StateChanged;

        private readonly IStateFactory<IState<TStateID>, TStateID> stateFactory;

        private IState<TStateID> currentState = default;
        private TStateID currentStateID;

        public TStateID CurrentStateID => currentStateID;

        public StateMachine(IStateFactory<IState<TStateID>, TStateID> factory)
        {
            if (factory == default)
            {
                Debug.LogError($"{nameof(StateMachine<TState, TStateID>)}: Trying to create state machine without state factory is definetily an error.");
                return;
            }

            stateFactory = factory;
        }

        public void Process(float deltaTime)
        {
            currentState?.Process(deltaTime);
        }

        public void SetState(TStateID newStateID, bool notify = true)
        {
            if (currentState != default)
            {
                if (Equals(currentStateID, newStateID))
                {
                    return;
                }

                currentState.Exit();
                currentState.StateChangeRequest -= HandleStateChangeRequest;
            }

            currentState = stateFactory.CreateState(newStateID);
            currentStateID = newStateID;

            if (currentState != default)
            {
                currentState.StateChangeRequest += HandleStateChangeRequest;
                currentState.Enter();
            }

            if (notify)
            {
                StateChanged?.Invoke(newStateID);
            }
        }

        private void HandleStateChangeRequest(TStateID newState)
        {
            SetState(newState);
        }
    }
}
