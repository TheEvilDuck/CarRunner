using Common.Sound;
using Common.States;
using DI;
using Gameplay.Cars;
using Levels;
using Services.PlayerInput;

namespace Gameplay.States
{
    public class RaceGameState : State
    {
        private readonly Timer _timer;
        private readonly CarBehaviour _car;
        private readonly SimpleCarCollisionTrigger _finish;
        private readonly SoundController _soundController;
        public RaceGameState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _timer = sceneContext.Get<Timer>();
            _car = sceneContext.Get<Car>().CarBehavior;
            _finish = sceneContext.Get<Level>().Finish;
            _soundController = sceneContext.Get<SoundController>();
        }

        public override void Update()
        {
            _timer.Update();
        }

        protected override void OnEnter()
        {
            _timer.Restart();
            _car.enabled = true;
            _soundController.Play(SoundID.BacgrondMusic);

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
