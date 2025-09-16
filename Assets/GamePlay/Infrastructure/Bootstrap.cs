using System;
using System.Collections;
using System.Linq;
using Common;
using Common.CameraFollow;
using Common.DeviceTypeHandling;
using Common.Disposables;
using Common.Mediators;
using Common.Reactive;
using Common.Settings;
using Common.Tickables;
using EntryPoint;
using GamePlay.CarFallingHangling;
using GamePlay.Cars.Scripts;
using GamePlay.Mediators;
using GamePlay.StateMachine.States;
using GamePlay.Timer;
using GamePlay.UI.Scripts;
using Infrastructure.Bootstraps;
using Infrastructure.DI;
using Levels.Scripts;
using Services.InputService;
using Services.InputService.Mobile;
using Services.Integrations;
using Services.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.Infrastructure
{
    public class Bootstrap : MonoBehaviourBootstrap
    {
        public const int WATCH_AD_REWAD_ID = 2;
        
        private const string RANGE_OF_CAMERA_SETTINGS_PATH = "Range Of Camera Settings";
        
        [SerializeField] private TimerView _timerView;
        [SerializeField] private Car _carPrefab;
        [SerializeField] private GameObject _wheelPrefab;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private Speedometr _speedometr;
        [SerializeField] private PauseButton _pauseButton;
        [SerializeField] private EndOfTheGame _endOfTheGame;
        [SerializeField] private SceneChangingButtons _pauseMenuButtons;
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private GameSettingsUI _settingsMenu;
        [SerializeField] private LayerMask _groundCheckLayer;
        [SerializeField] private Image _anticlicker;
        [SerializeField] private StartMessage _startMessage;
        [SerializeField] private Transform _brakeButtonParent;
        [SerializeField] private CarFallingView _carFallingView;

        public override void MakeRegistrationsInto(DIContainer container)
        {
            container.Register(() => new CompositeDisposable(), GameplayTags.DISPOSABLES);
            container.Register(() => new PauseLocker(container.Get<PauseManager>()));
            container.Register(() => SetUpLevel(container));
            container.Register(() => new Timer.Timer(container.Get<Level>().StartTimer));
            container.Register(() => SetUpCar(container));

            container.Register(() => SetupCarFalling(container))
                .AddToTickables(GameplayTags.TICKABLES);
            
            container.Register(() => new FallingTeleport(container.Get<Car>()));
            container.Register(() => new FallingEndGame());

            container.Register(() => SetUpFallingBehaviourSwitcher(container))
                .AddToDisposables(GameplayTags.DISPOSABLES);
            
            container.Register(() => new FallTries(container.Get<IPlayerData>().MaxFallTries));

            container.Register(() => SetUpGameplayStateMachine(container))
                .AddToDisposables(GameplayTags.DISPOSABLES)
                .AddToTickables(GameplayTags.TICKABLES);
            
            container.Register(_settingsMenu);
            container.Register(_timerView);
            container.Register(_endOfTheGame);
            container.Register(_pauseButton);
            container.Register(SetUpPauseMenu);
            container.Register(_pauseMenuButtons);
            container.Register(_anticlicker, "anticlicker");
            container.Register(Resources.Load<RangeOfCameraSettings>(RANGE_OF_CAMERA_SETTINGS_PATH));
            container.Register(() => Camera.main);
            container.Register(_cameraFollow);
            container.Register(_startMessage);
            container.Register(() => new Observable<CarConfig>());
            container.Register(_carFallingView);
            container.Register(() => SetupInputFactory(container));
            container.Register(SetupBrakeButtonFactory);

            container.Register(SetupTickables, GameplayTags.TICKABLES)
                .AddToTickables(EntryPointTags.PROJECT_TICKABLES_TAG);
            
            container.Register<IReadonlyObservable<CarConfig>>(() => container.Get<Observable<CarConfig>>())
                .NonLazy();
            
            container.Register(() => SetUpCarSwitcher(container))
                .AddToDisposables(GameplayTags.DISPOSABLES)
                .NonLazy();
            
            container.Register(() => SetUpPause(container), GameplayTags.PAUSE_MANAGER).NonLazy();
        }

        public override IEnumerator Initialize(IDIContainer container)
        {
            SetupInputs(container);
            SetUpCamera(container);
            SetUpPause(container);
            SetUpMediators(container);
            SetUpUI(container);

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
        
        private TickableManager SetupTickables() => new TickableManager();
        
        private IBrakeButtonFactory SetupBrakeButtonFactory() => new BrakeButtonFactory(_brakeButtonParent);
        
        private IInputSourceFactory SetupInputFactory(IDIContainer container)
        {
            IDeviceTypeHandler deviceTypeHandler = container.Get<IDeviceTypeHandler>();
            
            return new InputSourceFactory(deviceTypeHandler);
        }

        private PauseMenu SetUpPauseMenu()
        {
            _pauseMenu.Resume();
            return _pauseMenu;
        }

        private Level SetUpLevel(IDIContainer container)
        {
            string selectedLevelId = container.Get<IPlayerData>().SelectedLevel;

            if (string.IsNullOrEmpty(selectedLevelId))
                selectedLevelId = container.Get<LevelsDatabase>().GetFirstLevel();

            var level = Instantiate(container.Get<LevelsDatabase>().GetLevel(selectedLevelId));
            level.transform.position = Vector3.zero;

            foreach(Garage.Scripts.Garage garage in level.Garages.ToArray())
            {
                garage.Init(_wheelPrefab, container.Get<IReadonlyObservable<CarConfig>>());
            }

            RenderSettings.skybox = level.Skybox;
            RenderSettings.ambientSkyColor = level.AmbientSkyColor;
            RenderSettings.ambientEquatorColor = level.AmbientEquatorColor;
            RenderSettings.ambientGroundColor = level.AmbientGroundColor;

            return level;
        }
        
        private CarFalling SetupCarFalling(IDIContainer container)
        {
            Car car = container.Get<Car>();
            float yPositionToTeleportOffset = container.Get<Level>().YPositionToTeleportOffset;
            return new CarFalling(car, _groundCheckLayer, yPositionToTeleportOffset);
        }

        private Car SetUpCar(IDIContainer container)
        {
            var level = container.Get<Level>();
            var car = Instantiate(_carPrefab, level.CarStartPosition, level.CarStartRotation, null);
            car.InitCar(level.StartCar, _wheelPrefab);
            container.Get<Observable<CarConfig>>().Value = level.StartCar;

            return car;
        }

        private CarSwitcher SetUpCarSwitcher(IDIContainer container)
        {
            var carSwitcher = new CarSwitcher(
                container.Get<Car>(),
                container.Get<Level>().Garages,
                container.Get<Timer.Timer>(), 
                _wheelPrefab, 
                container.Get<Observable<CarConfig>>());
            
            return carSwitcher;
        }

        private FallingBehaviourSwitcher SetUpFallingBehaviourSwitcher(IDIContainer container)
        {
            var fallingBehaviourSwitcher = new FallingBehaviourSwitcher(container.Get<CarFalling>());
            fallingBehaviourSwitcher.AttachBehaviour(container.Get<FallingTeleport>());

            return fallingBehaviourSwitcher;
        }

        private void SetUpUI(IDIContainer container)
        {
            _speedometr.Init(container.Get<Car>().CarBehavior);
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

        private Common.States.StateMachine SetUpGameplayStateMachine(IDIContainer container)
        {
            var gameplayStateMachine = new Common.States.StateMachine();

            PreStartState preStartState = new PreStartState(gameplayStateMachine, container);
            RaceGameState raceGameState = new RaceGameState(gameplayStateMachine, container);
            WinState winState = new WinState(gameplayStateMachine, container);
            LoseState loseState = new LoseState(gameplayStateMachine, container);

            gameplayStateMachine.AddState(preStartState);
            gameplayStateMachine.AddState(raceGameState);
            gameplayStateMachine.AddState(winState);
            gameplayStateMachine.AddState(loseState);

            //Эта грязнь здесь, чтобы избежать циклическую зависимость
            container.Get<PauseManager>(GameplayTags.PAUSE_MANAGER).Register(gameplayStateMachine);

            return gameplayStateMachine;
        }
        
        private void SetUpCamera(IDIContainer container)
        {
            var car = container.Get<Car>();
            _cameraFollow.transform.position = car.transform.position;
            _cameraFollow.SetTarget(car.transform);
        }
        
        private PauseManager SetUpPause(IDIContainer container)
        {
            var pauseManager = new PauseManager();
            pauseManager.Register(container.Get<Timer.Timer>());
            pauseManager.Register(container.Get<Car>());
            pauseManager.Register(container.Get<StartMessage>());
            pauseManager.Register(container.Get<PauseMenu>());
            pauseManager.Register(container.Get<PauseLocker>());

            container.Get<PauseManager>().Register(pauseManager);

            return pauseManager;
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