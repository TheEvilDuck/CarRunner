using Common.Sound;
using Common.States;
using DI;
using Gameplay.Cars;
using Levels;

namespace Gameplay.States
{
    public class RaceGameState : State
    {
        private readonly Timer _timer;
        private readonly CarBehaviour _car;
        private readonly SimpleCarCollisionTrigger _finish;
        private readonly SoundController _soundController;
        private readonly Level _level;
        public RaceGameState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _timer = sceneContext.Get<Timer>();
            _car = sceneContext.Get<Car>().CarBehavior;
            _level = sceneContext.Get<Level>();
            _finish = _level.Finish;
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
            _soundController.Play(_level.BackGroundMusicId);

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
