using Common;
using Common.Sound;
using Common.States;
using DI;
using Gameplay.CarFallingHandling;
using Gameplay.Cars;
using Gameplay.UI;
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
        private readonly FallingEndGame _fallingEndGame;
        private readonly PauseManager _scenePause;
        private readonly PauseManager _projectPause;
        private readonly StartMessage _startMessage;
        public RaceGameState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _timer = sceneContext.Get<Timer>();
            _car = sceneContext.Get<Car>().CarBehavior;
            _level = sceneContext.Get<Level>();
            _finish = _level.Finish;
            _fallingEndGame = sceneContext.Get<FallingEndGame>();
            _soundController = sceneContext.Get<SoundController>();
            _scenePause = sceneContext.Get<PauseManager>(Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG);
            _projectPause = sceneContext.Get<PauseManager>();
            _startMessage = sceneContext.Get<StartMessage>();
        }

        public override void Update()
        {
            _timer.Update();
        }

        protected override void OnEnter()
        {
            _timer.Restart();
            _car.enabled = true;
            
            _projectPause.IsPaused.changed += OnScenePauseChanged;

            OnScenePauseChanged(_projectPause.IsPaused.Value);

            _timer.end+=Lose;
            _fallingEndGame.falled += Lose;
            _finish.passed += Win;

            _scenePause.Unregister(_startMessage);
        }
        protected override void OnExit()
        {
            _timer.end-=Lose;
            _finish.passed -= Win;
            _fallingEndGame.falled -= Lose;
            _projectPause.IsPaused.changed -= OnScenePauseChanged;
        }

        private void Lose()
        {
            _stateMachine.ChangeState<LoseState>();
        }

        private void Win()
        {
            _stateMachine.ChangeState<WinState>();
        }

        private void OnScenePauseChanged(bool isPaused)
        {
            if (isPaused)
                return;

            if (!_soundController.IsPlaying(_level.BackGroundMusicId))
                _soundController.Play(_level.BackGroundMusicId);
        }
    }
}
