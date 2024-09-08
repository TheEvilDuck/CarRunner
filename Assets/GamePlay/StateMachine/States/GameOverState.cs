using Common;
using Common.States;
using DI;
using Gameplay.Cars;
using Gameplay.UI;
using Services.PlayerInput;
using UnityEngine;
using YG;

namespace Gameplay.States
{
    public abstract class GameOverState : State
    {
        protected readonly DIContainer _sceneContext;
        public GameOverState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _sceneContext = sceneContext;
        }

        public override void Update()
        {
            _sceneContext.Get<Car>().CarBehavior.Brake(true);
        }

        protected override void OnEnter()
        {
            YandexGame.GameplayStop();
            
            PauseManager scenePause = _sceneContext.Get<PauseManager>(Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG);
            PauseManager globalPause = _sceneContext.Get<PauseManager>();

            PauseMenu pauseMenu = _sceneContext.Get<PauseMenu>();
            PauseButton pauseButton = _sceneContext.Get<PauseButton>();
            Car car = _sceneContext.Get<Car>();
            PauseLocker pauseLocker = _sceneContext.Get<PauseLocker>();
            YandexGameGameplay yandexGameGameplay = _sceneContext.Get<YandexGameGameplay>();
            EndOfTheGame endOfTheGame = _sceneContext.Get<EndOfTheGame>();
            IPlayerInput playerInput = _sceneContext.Get<IPlayerInput>();

            scenePause.Unregister(pauseMenu);
            scenePause.Unregister(pauseButton);
            scenePause.Unregister(car);
            scenePause.Unregister(pauseLocker);
            scenePause.Unregister(yandexGameGameplay);
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