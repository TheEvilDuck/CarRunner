using Common.States;
using Gameplay.Cars;

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

        public override void Update()
        {
            _timer.Update();
        }

        protected override void OnEnter()
        {
            _timer.Restart();
            _car.enabled = true;

            _timer.end+=OnTimerEnd;
        }
        protected override void OnExit()
        {
            _timer.end-=OnTimerEnd;
        }

        private void OnTimerEnd()
        {
            _stateMachine.ChangeState<GameoverState>();
        }
    }
}
