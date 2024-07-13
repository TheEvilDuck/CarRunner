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
using Gameplay.CarFallingHandling;
using EntryPoint;
using Common.Mediators;
using Common.Data;

namespace Gameplay
{
    public class Bootstrap : MonoBehaviourBootstrap
    {
        [SerializeField] private TimerView _timerView;
        [SerializeField] private Car _carPrefab;
        [SerializeField] private GameObject _wheelPrefab;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private Speedometr _speedometr;
        [SerializeField] private PauseButton _pauseButton;
        [SerializeField] private EndOfTheGame _endOfTheGame;
        [SerializeField] private SceneChangingButtons _pauseMenuButtons;
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private SettingsMenu _settingsMenu;
        [SerializeField] private LayerMask _groundCheckLayer;
        private List<IDisposable> _disposables;

        private void Start() 
        {
            _settingsMenu.Init(_sceneContext.Get<GameSettings>());
            var settingsAndSoundMediator = new SettingsAndSoundMediator(_sceneContext);
            _disposables.Add(settingsAndSoundMediator);
        }

        protected override void Setup()
        {
            _disposables = new List<IDisposable>();

            _sceneContext.Register(SetUpLevel);
            _sceneContext.Register(() => new Timer(_sceneContext.Get<Level>().StartTimer));
            _sceneContext.Register(SetUpCar);
            _sceneContext.Register(() => new CarFalling(_sceneContext.Get<Car>(), _groundCheckLayer));
            _sceneContext.Register(() => new FallingTeleport(_sceneContext.Get<Car>()));
            _sceneContext.Register(() => new FallingEndGame(_sceneContext.Get<StateMachine>()));
            _sceneContext.Register(SetUpFallingBehaviourSwitcher);
            _sceneContext.Register(() => new FallTries(3));
            _sceneContext.Register(SetUpGameplayStateMachine);
            _sceneContext.Register(_settingsMenu);
            _sceneContext.Register(SetUpPause);
            _sceneContext.Register(_timerView);
            _sceneContext.Register(_endOfTheGame);
            _sceneContext.Register(_pauseButton);
            _sceneContext.Register(_pauseMenu);
            _sceneContext.Register(_pauseMenuButtons);

            SetUpCarSwitcher();
            SetUpMediators();
            SetUpCamera();
            SetUpUI();
        }

        private void Update() 
        {
            _sceneContext.Get<StateMachine>().Update();
            _sceneContext.Get<IPlayerInput>().Update();
            _sceneContext.Get<CarFalling>().Update();
        }

        private void OnDestroy() 
        {
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();

            _disposables.Clear();

            _sceneContext.Get<SoundController>().Stop(SoundID.BacgrondMusic);
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
                garage.Init();
            }

            return level;
        }

        private Car SetUpCar()
        {
            var level = _sceneContext.Get<Level>();
            var car = Instantiate(_carPrefab, level.CarStartPosition, level.CarStartRotation, null);
            car.InitCar(level.StartCar, _wheelPrefab);

            return car;
        }

        private void SetUpCarSwitcher()
        {
            var carSwitcher = new CarSwitcher(_sceneContext.Get<Car>(),_sceneContext.Get<Level>().Garages,_sceneContext.Get<Timer>(), _wheelPrefab);
            _disposables.Add(carSwitcher);
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

            return gameplayStateMachine;
        }

        private PauseManager SetUpPause()
        {
            var pauseManager = new PauseManager();
            pauseManager.Register(_sceneContext.Get<Timer>());
            pauseManager.Register(_sceneContext.Get<Car>());
            pauseManager.Register(_sceneContext.Get<SoundController>());
            pauseManager.Register(_sceneContext.Get<StateMachine>());

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
            var settingMediator = new SettingsMediator(_sceneContext);
            var carFallingMediator = new CarFallingMediator(_sceneContext);
            var adButtonMediator = new AdButtonMediator(_sceneContext);

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
        }

        private void SetUpCamera()
        {
            var car = _sceneContext.Get<Car>();
            _cameraFollow.transform.position = car.transform.position;
            _cameraFollow.SetTarget(car.transform);
        }
    }
}
