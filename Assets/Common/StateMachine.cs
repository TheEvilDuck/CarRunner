using System;
using System.Collections.Generic;

namespace Common.States
{
    public class StateMachine
    {
        private Dictionary<Type,State> _states;
        private State _currentState;

        public StateMachine()
        {
            _states = new Dictionary<Type, State>();
        }

        public void AddState(State state)
        {
            if (!_states.TryAdd(state.GetType(),state))
                throw new Exception("You're trying add state of type already existing in state machine");

            if (_currentState==null)
            {
                _currentState = state;
                _currentState.Enter();
            }
        }

        public void ChangeState<TypeOfState>() where TypeOfState: State
        {
            if (!_states.TryGetValue(typeof(TypeOfState), out State state))
                throw new ArgumentException($"There is no state of {typeof(TypeOfState)} in state machine");

            _currentState?.Exit();
            _currentState = state;
            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }
    }
}
