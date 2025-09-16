using Common;
using Common.States;
using GamePlay.Cars.Scripts;
using GamePlay.Infrastructure;
using GamePlay.UI.Scripts;
using Infrastructure.DI;
using Services.InputService;

namespace GamePlay.StateMachine.States
{
    public abstract class GameOverState : State
    {
        protected readonly IDIContainer _sceneContext;
        public GameOverState(Common.States.StateMachine stateMachine, IDIContainer sceneContext) : base(stateMachine)
        {
            _sceneContext = sceneContext;
        }

        public override void Update()
        {
            _sceneContext.Get<Car>().CarBehavior.Brake(true);
        }

        protected override void OnEnter()
        {
            PauseManager scenePause = _sceneContext.Get<PauseManager>(GameplayTags.PAUSE_MANAGER);
            PauseManager globalPause = _sceneContext.Get<PauseManager>();

            PauseMenu pauseMenu = _sceneContext.Get<PauseMenu>();
            PauseButton pauseButton = _sceneContext.Get<PauseButton>();
            Car car = _sceneContext.Get<Car>();
            PauseLocker pauseLocker = _sceneContext.Get<PauseLocker>();
            EndOfTheGame endOfTheGame = _sceneContext.Get<EndOfTheGame>();
            PlayerInput playerInput = _sceneContext.Get<PlayerInput>();

            scenePause.Unregister(pauseMenu);
            scenePause.Unregister(pauseButton);
            scenePause.Unregister(car);
            scenePause.Unregister(pauseLocker);
            scenePause.Unregister(_stateMachine);

            scenePause.Pause();
            scenePause.Lock();
            globalPause.Unlock();

            pauseMenu.Resume();
            pauseButton.Hide();

            globalPause.Unregister(scenePause);

            endOfTheGame.Show();
            car.CarBehavior.enabled = false;
            playerInput.Disable();
        }

        protected override void OnExit()
        {
            _sceneContext.Get<PauseManager>().Resume();
        }
    }
}