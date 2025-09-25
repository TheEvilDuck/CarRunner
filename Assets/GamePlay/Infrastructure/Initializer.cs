using System.Collections;
using Common;
using Common.CameraFollow;
using Common.Disposables;
using Common.Mediators;
using Common.Settings;
using GamePlay.Cars.Scripts;
using GamePlay.Mediators;
using GamePlay.Timer;
using GamePlay.UI.Scripts;
using Infrastructure.Bootstraps;
using Infrastructure.DI;
using Services.InputService;
using Services.Integrations;
using UnityEngine;

namespace GamePlay.Infrastructure
{
    public class Initializer: IBootstrapInitializer
    {
        private readonly Speedometr _speedometer;
        private readonly GameSettingsUI _settingsMenu;
        private CameraFollow _cameraFollow;

        public Initializer(
            Speedometr speedometer, 
            GameSettingsUI settingsMenu, 
            CameraFollow cameraFollow)
        {
            _speedometer = speedometer;
            _settingsMenu = settingsMenu;
            _cameraFollow = cameraFollow;
        }

        public IEnumerator Initialize(IDIContainer container)
        {
            SetupInputs(container);
            SetUpCamera(container);
            SetUpMediators(container);
            SetUpUI(container);
            RegisterPausables(container);

            DisposableDelegate pauseDisposing = new DisposableDelegate(() =>
            {
                PauseManager scenePause = container.Get<PauseManager>(GameplayTags.PAUSE_MANAGER);
                PauseManager globalPause = container.Get<PauseManager>();

                PauseLocker pauseLocker = container.Get<PauseLocker>();
                scenePause.Unregister(pauseLocker);
                globalPause.Unlock();
                globalPause.Unregister(scenePause);
            });
            
            container.Get<CompositeDisposable>(GameplayTags.DISPOSABLES).Add(pauseDisposing);

            yield return new WaitForEndOfFrame();
            
            container.Get<YandexGameIntegrator>().MarkGameplay(true);
        }
        
        private void SetUpUI(IDIContainer container)
        {
            _speedometer.Init(container.Get<Car>().CarBehavior);
            _settingsMenu.Init(container.Get<ICameraSettings>(), container.Get<ISoundSettings>());
        }
        
        private void SetupInputs(IDIContainer container)
        {
            IInputSourceFactory factory = container.Get<IInputSourceFactory>();
            PlayerInput playerInput = container.Get<PlayerInput>();
            IInputSource inputSource = factory.Get();
            
            playerInput.SwitchInputSource(inputSource);
            
            playerInput.Enable();
        }
        
        private void SetUpCamera(IDIContainer container)
        {
            var car = container.Get<Car>();
            _cameraFollow.transform.position = car.transform.position;
            _cameraFollow.SetTarget(car.transform);
        }
        
        private void RegisterPausables(IDIContainer container)
        {
            PauseManager pauseManager = container.Get<PauseManager>(GameplayTags.PAUSE_MANAGER);
            
            pauseManager.Register(container.Get<Timer.Timer>());
            pauseManager.Register(container.Get<Car>());
            pauseManager.Register(container.Get<StartMessage>());
            pauseManager.Register(container.Get<PauseMenu>());
            pauseManager.Register(container.Get<PauseLocker>());
            pauseManager.Register(container.Get<Common.States.StateMachine>());
        }
        
        private void SetUpMediators(IDIContainer container)
        {
            var gameplayMarkingMediator = new GameplayMarkingMediator(container);
            var timerMediator = new TimerMediator(container);
            var carControllerMediator = new CarControllerMediator(container);
            var timerAndGatesMediator = new TimerAndGatesMediator(container);
            var soundMediator = new SoundMediator(container);
            var endGameMediator = new EndGameMediator(container);
            var pauseMediator = new PauseMediator(container);
            var pauseMenuMediator = new PauseMenuMediator(container);
            var settingMediator = new SettingsAndUIMediator(container);
            var carFallingMediator = new CarFallingMediator(container);
            var adButtonMediator = new AdButtonMediator(container);
            var settingsAndCameraMediator = new SettingsAndCameraMediator(container);    
            var settingsAndSoundMediator = new SettingsAndSoundMediator(container);
            var gameplayTickablesCleanup = new GameplayTickablesCleanup(container);
            var onSceneChangedDisposeContextMediator = new OnSceneChangedDisposeContextMediator(container, GameplayTags.DISPOSABLES);
            
            var disposables = container.Get<CompositeDisposable>(GameplayTags.DISPOSABLES);        

            disposables.Add(timerMediator);
            disposables.Add(carControllerMediator);
            disposables.Add(timerAndGatesMediator);
            disposables.Add(soundMediator);
            disposables.Add(endGameMediator);
            disposables.Add(pauseMediator);
            disposables.Add(pauseMenuMediator);
            disposables.Add(settingMediator);
            disposables.Add(carFallingMediator);
            disposables.Add(adButtonMediator);
            disposables.Add(settingsAndCameraMediator);
            disposables.Add(settingsAndSoundMediator);
            disposables.Add(onSceneChangedDisposeContextMediator);
            disposables.Add(gameplayMarkingMediator);
            disposables.Add(gameplayTickablesCleanup);
        }
    }
}