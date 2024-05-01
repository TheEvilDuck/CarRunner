using System;

namespace Common.States
{
    public abstract class State
    {
        public event Action entered;
        public event Action exited;
        protected StateMachine _stateMachine;
        public State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public void Enter()
        {
            entered?.Invoke();
            OnEnter();
        }
        public void Exit()
        {
            exited?.Invoke();
            OnExit();
        }
        public virtual void Update() {}

        protected virtual void OnEnter() {}
        protected virtual void OnExit () {}
    }
}
