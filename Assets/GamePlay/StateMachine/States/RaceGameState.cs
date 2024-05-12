using Common.States;
using Gameplay.Cars;

namespace Gameplay.States
{
    public class RaceGameState : State
    {
        private Timer _timer;
        private readonly CarBehaviour _car;
        private readonly SimpleCarCollisionTrigger _finish;
        public RaceGameState(StateMachine stateMachine, Timer timer, CarBehaviour carBehaviour, SimpleCarCollisionTrigger finish) : base(stateMachine)
        {
            _timer = timer;
            _car = carBehaviour;
            _finish = finish;
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
            _finish.passed += OnLevelFinished;
        }
        protected override void OnExit()
        {
            _timer.end-=OnTimerEnd;
            _finish.passed -= OnLevelFinished;
        }

        private void OnTimerEnd()
        {
            _stateMachine.ChangeState<LoseState>();
        }

        private void OnLevelFinished()
        {
            _stateMachine.ChangeState<WinState>();
        }
    }
}
