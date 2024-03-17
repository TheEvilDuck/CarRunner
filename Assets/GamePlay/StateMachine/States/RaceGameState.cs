using Common.States;

namespace Gameplay.States
{
    public class RaceGameState : State
    {
        private Timer _timer;
        public RaceGameState(StateMachine stateMachine, Timer timer) : base(stateMachine)
        {
            _timer = timer;
        }

        public override void Enter()
        {
            _timer.Restart();

            _timer.end+=OnTimerEnd;
        }

        public override void Exit()
        {
            _timer.end-=OnTimerEnd;
        }

        public override void Update()
        {
            _timer.Update();
        }

        private void OnTimerEnd()
        {
            _stateMachine.ChangeState<GameoverState>();
        }
    }
}
