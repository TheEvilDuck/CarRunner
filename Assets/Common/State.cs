namespace Common.States
{
    public abstract class State
    {
        protected StateMachine _stateMachine;
        public State(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        public virtual void Enter() {}
        public virtual void Exit() {}
        public virtual void Update() {}
    }
}
