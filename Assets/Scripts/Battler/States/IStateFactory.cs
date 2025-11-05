using System;

namespace AutoBattler
{
    public interface IStateFactory<out TState, in TStateID>
        where TState: IState
        where TStateID: Enum
    {
        public TState CreateState(TStateID stateID);
    }
}