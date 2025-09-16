using Common;
using Common.Sounds.Scripts;
using Common.States;
using GamePlay.CarFallingHangling;
using GamePlay.Cars.Scripts;
using GamePlay.Infrastructure;
using GamePlay.UI.Scripts;
using Infrastructure.DI;
using Levels.Scripts;

namespace GamePlay.StateMachine.States
{
    public class RaceGameState : State
    {
        private readonly Timer.Timer _timer;
        private readonly CarBehaviour _car;
        private readonly SimpleCarCollisionTrigger _finish;
        private readonly ISoundController _soundController;
        private readonly Level _level;
        private readonly FallingEndGame _fallingEndGame;
        private readonly PauseManager _scenePause;
        private readonly PauseManager _projectPause;
        private readonly StartMessage _startMessage;
        private readonly PauseButton _pauseButton;

        public RaceGameState(Common.States.StateMachine stateMachine, IDIContainer sceneContext) : base(stateMachine)
        {
            _timer = sceneContext.Get<Timer.Timer>();
            _car = sceneContext.Get<Car>().CarBehavior;
            _level = sceneContext.Get<Level>();
            _finish = _level.Finish;
            _fallingEndGame = sceneContext.Get<FallingEndGame>();
            _soundController = sceneContext.Get<ISoundController>();
            _scenePause = sceneContext.Get<PauseManager>(GameplayTags.PAUSE_MANAGER);
            _projectPause = sceneContext.Get<PauseManager>();
            _startMessage = sceneContext.Get<StartMessage>();
            _pauseButton = sceneContext.Get<PauseButton>();
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
            _scenePause.Register(_pauseButton);
        }
        protected override void OnExit()
        {
            _timer.end-=Lose;
            _finish.passed -= Win;
            _fallingEndGame.falled -= Lose;
            _projectPause.IsPaused.changed -= OnScenePauseChanged;
            _pauseButton.Hide();
            _scenePause.Unregister(_pauseButton);
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

            if (!_soundController.IsSoundExisting(_level.BackGroundMusicId))
                _soundController.Play(_level.BackGroundMusicId);
        }
    }
}
