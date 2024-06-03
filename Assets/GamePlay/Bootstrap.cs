using System.Collections.Generic;
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

namespace Gameplay
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private float _startTime = 20f;
        [SerializeField]private TimerView _timerView;
        [SerializeField] private LevelsDatabase _levels;
        [SerializeField]private Car _carPrefab;
        [SerializeField]private CarConfig _startConfig;
        [SerializeField]private GameObject _wheelPrefab;
        [SerializeField] private string _defaultLevelId;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField]private SoundController _soundController;
        private Timer _timer;
        private List<IPausable>_pausableControls;
        private TimerMediator _timerMediator;
        private TimerAndGatesMediator _timerAndGatesMediator;
        private CarControllerMediator _carControllerMediator;
        private StateMachine _gameplayStateMachine;
        private IPlayerInput _playerInput;
        private CarSwitcher _carSwitcher;
        private PlayerData _playerData;
        private Level _level;
        private Car _car;
        private SoundMediator _soundMediator;

        private void Awake() 
        {
            if (SystemInfo.deviceType== DeviceType.Desktop)
                _playerInput = new DesktopInput();
            else if (SystemInfo.deviceType == DeviceType.Handheld)
                _playerInput = new MobileInput();

            _playerData = new PlayerData();
            string levelId = _playerData.SelectedLevel;

            if (string.IsNullOrEmpty(levelId))
                levelId = _defaultLevelId;
            
            _level = Instantiate(_levels.GetLevel(levelId));
            _level.transform.position = Vector3.zero;
            

            _timer = new Timer(_startTime);
            _pausableControls = new List<IPausable>
            {
                _timer
            };

            _timerMediator = new TimerMediator(_timer, _timerView);

            _gameplayStateMachine = new StateMachine();

            _car = Instantiate(_carPrefab);

            PreStartState preStartState = new PreStartState(_gameplayStateMachine);
            RaceGameState raceGameState = new RaceGameState(_gameplayStateMachine, _timer, _car.CarBehavior, _level.Finish);
            WinState winState = new WinState(_gameplayStateMachine, _car.CarBehavior);
            LoseState loseState = new LoseState(_gameplayStateMachine, _car.CarBehavior);


            _gameplayStateMachine.AddState(preStartState);
            _gameplayStateMachine.AddState(raceGameState);
            _gameplayStateMachine.AddState(winState);
            _gameplayStateMachine.AddState(loseState);

            _timerAndGatesMediator = new TimerAndGatesMediator(_level.TimerGates.ToArray(), _timer);

            _carControllerMediator = new CarControllerMediator(_car.CarBehavior, _playerInput);

            foreach(Garage garage in _level.Garages.ToArray())
            {
                garage.Init(_timer);
            }

            _carSwitcher = new CarSwitcher(_car,_level.Garages.ToArray(),_timer, _wheelPrefab);

            _car.InitCar(_startConfig, _wheelPrefab);

            _car.transform.position = _level.CarStartPosition;
            _cameraFollow.SetTarget(_car.transform);
            _car.transform.position = _level.CarStartPosition;


            _soundController.Init();
            _soundMediator = new SoundMediator(_soundController, _timerGates, _garages, raceGameState);
        }

        private void Update() 
        {
            _gameplayStateMachine.Update();
            _playerInput.Update();
        }

        private void OnDestroy() 
        {
            _timerMediator.Dispose();
            _timerAndGatesMediator.Dispose();
            _carControllerMediator.Dispose();
            _carSwitcher.Dispose();
            _gameplayStateMachine.Dispose();
            _soundMediator.Dispose();
        }
    }
}
