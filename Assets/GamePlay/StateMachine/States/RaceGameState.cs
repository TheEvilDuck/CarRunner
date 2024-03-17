using Common.States;

namespace Gameplay.States
{
    public class RaceGameState : State
    {
        private Timer _timer;
        private readonly CarBehaviour _car;
        public RaceGameState(StateMachine stateMachine, Timer timer, CarBehaviour carBehaviour) : base(stateMachine)
        {
            _timer = timer;
            _car = carBehaviour;
        }

        public override void Enter()
        {
            _timer.Restart();
            _car.enabled = true;

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
