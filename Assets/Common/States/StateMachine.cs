using System;
using System.Collections.Generic;

namespace Common.States
{
    public class StateMachine: IDisposable, IPausable
    {
        private Dictionary<Type,State> _states;
        private State _currentState;
        private bool _paused;

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

        public State GetState<TypeOfState>() where TypeOfState: State
        {
            if (!_states.TryGetValue(typeof(TypeOfState), out State state))
                throw new ArgumentException($"There is no state of {typeof(TypeOfState)} in state machine");

            return state;
        }

        public void Dispose() => _currentState?.Exit();
        public void Update()
        {
            if (_paused)
                return;

            _currentState?.Update();
        }

        public void Pause() => _paused = true;

        public void Resume() => _paused = false;
    }
}
