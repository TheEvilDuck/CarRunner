using System.Linq;
using Common;
using Common.Sound;
using Common.States;
using Gameplay.Cars;
using Gameplay.Garages;
using Levels;
using Gameplay.States;
using Gameplay.TimerGates;
using Gameplay.UI;
using Services.PlayerInput;
using UnityEngine;
using Common.Components;
using System.Collections.Generic;
using System;
using Gameplay.CarFallingHandling;
using EntryPoint;
using Common.Mediators;
using Common.Data;
using YG;
using UnityEngine.UI;
using Common.UI;
using Common.Reactive;

namespace Gameplay
{
    public class Bootstrap : MonoBehaviourBootstrap
    {
        public const string GAMEPLAY_PAUSE_MANAGER_TAG = "Gameplay pause";
        private const string RANGE_OF_CAMERA_SETTINGS_PATH = "Range Of Camera Settings";
        public const int WATCH_AD_REWAD_ID = 2;
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
        private List<IDisposable> _disposables;

        protected override void Setup()
        {
            _disposables = new List<IDisposable>();

            _sceneContext.Get<YandexGameFullScreenAd>().ShowFullscreenAd();

            _sceneContext.Register(() => new PauseLocker(_sceneContext.Get<PauseManager>()));
            _sceneContext.Register(() => new YandexGameGameplay());
            _sceneContext.Register(SetUpLevel);
            _sceneContext.Register(() => new Timer(_sceneContext.Get<Level>().StartTimer));
            _sceneContext.Register(SetUpCar);
            _sceneContext.Register(() => new CarFalling(_sceneContext.Get<Car>(), _groundCheckLayer));
            _sceneContext.Register(() => new FallingTeleport(_sceneContext.Get<Car>()));
            _sceneContext.Register(() => new FallingEndGame());
            _sceneContext.Register(SetUpFallingBehaviourSwitcher);
            _sceneContext.Register(() => new FallTries(_sceneContext.Get<IPlayerData>().MaxFallTries));
            _sceneContext.Register(SetUpGameplayStateMachine);
            _sceneContext.Register(_settingsMenu);
            _sceneContext.Register(_timerView);
            _sceneContext.Register(_endOfTheGame);
            _sceneContext.Register(_pauseButton);
            _sceneContext.Register(SetUpPauseMenu);
            _sceneContext.Register(_pauseMenuButtons);
            _sceneContext.Register(_anticlicker, "anticlicker");
            _sceneContext.Register(Resources.Load<RangeOfCameraSettings>(RANGE_OF_CAMERA_SETTINGS_PATH));
            _sceneContext.Register(() => Camera.main);
            _sceneContext.Register(_cameraFollow);
            _sceneContext.Register(_startMessage);
            _sceneContext.Register(() => new Observable<CarConfig>());
            _sceneContext.Register(_carFallingView);
            _sceneContext.Register<IReadonlyObservable<CarConfig>>(() => _sceneContext.Get<Observable<CarConfig>>()).NonLazy();
            _sceneContext.Register(SetUpCarSwitcher).NonLazy();
            _sceneContext.Register(SetUpPause, GAMEPLAY_PAUSE_MANAGER_TAG).NonLazy();
            
            SetUpMediators();
            SetUpCamera();
            SetUpUI();

            if (Application.isFocused)
                _sceneContext.Get<PauseManager>().Resume();
            else
                _sceneContext.Get<PauseManager>().Pause();

            _delayedStart += OnDelayedStart;
        }

        private void Update() 
        {
            if (!_inited)
                return;

            _sceneContext.Get<IPlayerInput>().Update();
            _sceneContext.Get<StateMachine>().Update();
            _sceneContext.Get<CarFalling>().Update();
        }
        
        protected override void OnBeforeSceneChanged()
        {
            base.OnBeforeSceneChanged();

            Debug.Log("Resources clean up");

            foreach (IDisposable disposable in _disposables)
            {
                Debug.Log($"Disposed {disposable.GetType()}");
                disposable?.Dispose();
            }

            _disposables.Clear();

            PauseManager scenePause = _sceneContext.Get<PauseManager>(Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG);
            PauseManager globalPause = _sceneContext.Get<PauseManager>();

            PauseLocker pauseLocker = _sceneContext.Get<PauseLocker>();
            scenePause.Unregister(pauseLocker);
            globalPause.Unlock();
            globalPause.Unregister(scenePause);

            Debug.Log("Resources clean up done!");
            
            YandexGame.GameplayStop();
        }

        private void OnDelayedStart()
        {
            _delayedStart -= OnDelayedStart;
            var settingsAndSoundMediator = new SettingsAndSoundMediator(_sceneContext);
            _disposables.Add(settingsAndSoundMediator);

            if (!_sceneContext.Get<PauseManager>().IsPaused.Value)
                YandexGame.GameplayStart();
        }

        private PauseMenu SetUpPauseMenu()
        {
            _pauseMenu.Resume();
            return _pauseMenu;
        }

        private Level SetUpLevel()
        {
            string selectedLevelId = _sceneContext.Get<IPlayerData>().SelectedLevel;

            if (string.Equals(selectedLevelId, string.Empty))
                selectedLevelId = _sceneContext.Get<LevelsDatabase>().GetFirstLevel();

            var level = Instantiate(_sceneContext.Get<LevelsDatabase>().GetLevel(selectedLevelId));
            level.transform.position = Vector3.zero;

            foreach(Garage garage in level.Garages.ToArray())
            {
                garage.Init(_wheelPrefab, _sceneContext.Get<IReadonlyObservable<CarConfig>>());
            }

            RenderSettings.skybox = level.Skybox;
            RenderSettings.ambientSkyColor = level.AmbientSkyColor;
            RenderSettings.ambientEquatorColor = level.AmbientEquatorColor;
            RenderSettings.ambientGroundColor = level.AmbientGroundColor;

            return level;
        }

        private Car SetUpCar()
        {
            var level = _sceneContext.Get<Level>();
            var car = Instantiate(_carPrefab, level.CarStartPosition, level.CarStartRotation, null);
            car.InitCar(level.StartCar, _wheelPrefab);
            _sceneContext.Get<Observable<CarConfig>>().Value = level.StartCar;

            return car;
        }

        private CarSwitcher SetUpCarSwitcher()
        {
            var carSwitcher = new CarSwitcher(_sceneContext.Get<Car>(),_sceneContext.Get<Level>().Garages,_sceneContext.Get<Timer>(), _wheelPrefab, _sceneContext.Get<Observable<CarConfig>>());
            _disposables.Add(carSwitcher);
            return carSwitcher;
        }

        private FallingBehaviourSwitcher SetUpFallingBehaviourSwitcher()
        {
            var fallingBehaviourSwitcher = new FallingBehaviourSwitcher(_sceneContext.Get<CarFalling>());
            fallingBehaviourSwitcher.AttachBehaviour(_sceneContext.Get<FallingTeleport>());

            _disposables.Add(fallingBehaviourSwitcher);

            return fallingBehaviourSwitcher;
        }

        private void SetUpUI()
        {
            _speedometr.Init(_sceneContext.Get<Car>().CarBehavior);
            _settingsMenu.Init(_sceneContext.Get<ICameraSettings>(), _sceneContext.Get<ISoundSettings>());
            
            if (_sceneContext.Get<DeviceType>() == DeviceType.Handheld)
                _sceneContext.Get<IBrakeButton>().SetParent(_brakeButtonParent);
        }

        private StateMachine SetUpGameplayStateMachine()
        {
            var gameplayStateMachine = new StateMachine();

            PreStartState preStartState = new PreStartState(gameplayStateMachine, _sceneContext);
            RaceGameState raceGameState = new RaceGameState(gameplayStateMachine, _sceneContext);
            WinState winState = new WinState(gameplayStateMachine, _sceneContext);
            LoseState loseState = new LoseState(gameplayStateMachine, _sceneContext);

            gameplayStateMachine.AddState(preStartState);
            gameplayStateMachine.AddState(raceGameState);
            gameplayStateMachine.AddState(winState);
            gameplayStateMachine.AddState(loseState);

            _disposables.Add(gameplayStateMachine);

            //Эта грязнь здесь, чтобы избежать циклическую зависимость
            _sceneContext.Get<PauseManager>(GAMEPLAY_PAUSE_MANAGER_TAG).Register(gameplayStateMachine);

            return gameplayStateMachine;
        }

        private PauseManager SetUpPause()
        {
            var pauseManager = new PauseManager();
            pauseManager.Register(_sceneContext.Get<Timer>());
            pauseManager.Register(_sceneContext.Get<Car>());
            pauseManager.Register(_sceneContext.Get<StartMessage>());
            pauseManager.Register(_sceneContext.Get<PauseMenu>());
            pauseManager.Register(_sceneContext.Get<PauseLocker>());
            pauseManager.Register(_sceneContext.Get<PauseButton>());
            pauseManager.Register(_sceneContext.Get<YandexGameGameplay>());

            _sceneContext.Get<PauseManager>().Register(pauseManager);

            return pauseManager;
        }

        private void SetUpMediators()
        {
            var timerMediator = new TimerMediator(_sceneContext);
            var carControllerMediator = new CarControllerMediator(_sceneContext);
            var timerAndGatesMediator = new TimerAndGatesMediator(_sceneContext);
            var soundMediator = new SoundMediator(_sceneContext);
            var endGameMediator = new EndGameMediator(_sceneContext);
            var pauseMediator = new PauseMediator(_sceneContext);
            var pauseMenuMediator = new PauseMenuMediator(_sceneContext);
            var settingMediator = new SettingsAndUIMediator(_sceneContext);
            var carFallingMediator = new CarFallingMediator(_sceneContext);
            var adButtonMediator = new AdButtonMediator(_sceneContext);
            var fullscreenAdMediator = new FullscreenAdMediator(_sceneContext);
            var settingsAndCameraMediator = new SettingsAndCameraMediator(_sceneContext);            

            _disposables.Add(timerMediator);
            _disposables.Add(carControllerMediator);
            _disposables.Add(timerAndGatesMediator);
            _disposables.Add(soundMediator);
            _disposables.Add(endGameMediator);
            _disposables.Add(pauseMediator);
            _disposables.Add(pauseMenuMediator);
            _disposables.Add(settingMediator);
            _disposables.Add(carFallingMediator);
            _disposables.Add(adButtonMediator);
            _disposables.Add(fullscreenAdMediator);
            _disposables.Add(settingsAndCameraMediator);
        }

        private void SetUpCamera()
        {
            var car = _sceneContext.Get<Car>();
            _cameraFollow.transform.position = car.transform.position;
            _cameraFollow.SetTarget(car.transform);
        }
    }
}