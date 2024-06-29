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
using MainMenu;

namespace Gameplay
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private TimerView _timerView;
        [SerializeField] private LevelsDatabase _levels;
        [SerializeField] private Car _carPrefab;
        [SerializeField] private GameObject _wheelPrefab;
        [SerializeField] private string _defaultLevelId;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private SoundController _soundController;
        [SerializeField] private Speedometr _speedometr;
        [SerializeField] private PauseButton _pauseButton;
        [SerializeField] private EndOfTheGame _endOfTheGame;
        [SerializeField] private SceneChangingButtons _pauseMenuButtons;
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private SettingsMenu _settingsMenu;
        private GameSettings _gameSettings;
        private List<IDisposable> _disposables;
        private PauseManager _pauseManager;
        private Timer _timer;
        private StateMachine _gameplayStateMachine;
        private IPlayerInput _playerInput;
        private CarSwitcher _carSwitcher;
        private PlayerData _playerData;
        private Level _level;
        private Car _car;
        private SceneLoader _sceneLoader;
        private string _levelId;

        private void Awake() 
        {
            _disposables = new List<IDisposable>();

            SetUpInput();
            SetUpPlayerData();
            SetUpLevel();
            SetUpCar();
            SetUpSubSystems();
            SetUpUI();
            SetUpStateMachine();
            SetUpPause();
            SetUpMediators();
            SetUpCamera();
        }

        private void Update() 
        {
            _gameplayStateMachine.Update();
            _playerInput.Update();
        }

        private void OnDestroy() 
        {
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();

            _disposables.Clear();
        }

        private void SetUpInput()
        {
            if (SystemInfo.deviceType== DeviceType.Desktop)
                _playerInput = new DesktopInput();
            else if (SystemInfo.deviceType == DeviceType.Handheld)
                _playerInput = new MobileInput();
        }

        private void SetUpPlayerData()
        {
            _playerData = new PlayerData();
            _levelId = _playerData.SelectedLevel;

            if (string.IsNullOrEmpty(_levelId))
                _levelId = _defaultLevelId;
        }

        private void SetUpLevel()
        {
            _level = Instantiate(_levels.GetLevel(_levelId));
            _level.transform.position = Vector3.zero;
            _timer = new Timer(_level.StartTimer);

            foreach(Garage garage in _level.Garages.ToArray())
            {
                garage.Init(_timer);
            }

            _level.Init();
        }

        private void SetUpCar()
        {
            _car = Instantiate(_carPrefab, _level.CarStartPosition, _level.CarStartRotation, null);
            _car.InitCar(_level.StartCar, _wheelPrefab);
            _carSwitcher = new CarSwitcher(_car,_level.Garages.ToArray(),_timer, _wheelPrefab);

            _disposables.Add(_carSwitcher);
        }

        private void SetUpStateMachine()
        {
            _gameplayStateMachine = new StateMachine();

            PreStartState preStartState = new PreStartState(_gameplayStateMachine, _car.CarBehavior);
            RaceGameState raceGameState = new RaceGameState(_gameplayStateMachine, _timer, _car.CarBehavior, _level.Finish);
            WinState winState = new WinState(_gameplayStateMachine, _car.CarBehavior);
            LoseState loseState = new LoseState(_gameplayStateMachine, _car.CarBehavior);

            _gameplayStateMachine.AddState(preStartState);
            _gameplayStateMachine.AddState(raceGameState);
            _gameplayStateMachine.AddState(winState);
            _gameplayStateMachine.AddState(loseState);

            _disposables.Add(_gameplayStateMachine);

        }

        private void SetUpSubSystems()
        {
            _soundController.Init();
            _sceneLoader = new SceneLoader();
            _gameSettings = new GameSettings();
            _gameSettings.LoadSettings();
        }

        private void SetUpPause()
        {
            _pauseManager = new PauseManager();
            _pauseManager.Register(_timer);
            _pauseManager.Register(_car);
            _pauseManager.Register(_soundController);
            _pauseManager.Register(_gameplayStateMachine);

            _settingsMenu.Init(_gameSettings);
        }

        private void SetUpUI()
        {
            _speedometr.Init(_car.CarBehavior);
        }

        private void SetUpMediators()
        {
            var timerMediator = new TimerMediator(_timer, _timerView);
            var carControllerMediator = new CarControllerMediator(_car.CarBehavior, _playerInput);
            var timerAndGatesMediator = new TimerAndGatesMediator(_level.TimerGates.ToArray(), _timer);
            var soundMediator = new SoundMediator(_soundController, _level.TimerGates.ToArray(), _level.Garages.ToArray(), _gameplayStateMachine.GetState<RaceGameState>(), _gameSettings);
            var endGameMediator = new EndGameMediator(_endOfTheGame, _sceneLoader, _gameplayStateMachine.GetState<LoseState>(), _gameplayStateMachine.GetState<WinState>(), _pauseButton, carControllerMediator);
            var pauseMediator = new PauseMediator(_pauseManager, _pauseButton, _pauseMenu);
            var pauseMenuMediator = new PauseMenuMediator(_pauseMenuButtons, _sceneLoader);
            var settingMediator = new SettingsMediator(_gameSettings, _settingsMenu);

            _disposables.Add(timerMediator);
            _disposables.Add(carControllerMediator);
            _disposables.Add(timerAndGatesMediator);
            _disposables.Add(soundMediator);
            _disposables.Add(endGameMediator);
            _disposables.Add(pauseMediator);
            _disposables.Add(pauseMenuMediator);
            _disposables.Add(settingMediator);
        }

        private void SetUpCamera()
        {
            _cameraFollow.transform.position = _car.transform.position;
            _cameraFollow.SetTarget(_car.transform);
        }
    }
}
