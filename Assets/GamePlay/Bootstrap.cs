using System.Collections.Generic;
using System.Linq;
using Common;
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
        [SerializeField] private string _testLevelId;
        [SerializeField] private CameraFollow _cameraFollow;
        private Timer _timer;
        private List<IPausable>_pausableControls;
        private TimerMediator _timerMediator;
        private TimerAndGatesMediator _timerAndGatesMediator;
        private CarControllerMediator _carControllerMediator;
        private StateMachine _gameplayStateMachine;
        private IPlayerInput _playerInput;
        private CarSwitcher _carSwitcher;

        private void Awake() 
        {
            if (SystemInfo.deviceType== DeviceType.Desktop)
                _playerInput = new DesktopInput();
            else if (SystemInfo.deviceType == DeviceType.Handheld)
                _playerInput = new MobileInput();

            Level testLevel = Instantiate(_levels.GetLevel(_testLevelId));
            testLevel.transform.position = Vector3.zero;

            _timer = new Timer(_startTime);
            _pausableControls = new List<IPausable>
            {
                _timer
            };

            _timerMediator = new TimerMediator(_timer, _timerView);

            _gameplayStateMachine = new StateMachine();

            var car = Instantiate(_carPrefab);

            PreStartState preStartState = new PreStartState(_gameplayStateMachine);
            RaceGameState raceGameState = new RaceGameState(_gameplayStateMachine, _timer, car.CarBehavior, testLevel.Finish);
            WinState winState = new WinState(_gameplayStateMachine, car.CarBehavior);
            LoseState loseState = new LoseState(_gameplayStateMachine, car.CarBehavior);


            _gameplayStateMachine.AddState(preStartState);
            _gameplayStateMachine.AddState(raceGameState);
            _gameplayStateMachine.AddState(winState);
            _gameplayStateMachine.AddState(loseState);

            _timerAndGatesMediator = new TimerAndGatesMediator(testLevel.TimerGates.ToArray(), _timer);

            _carControllerMediator = new CarControllerMediator(car.CarBehavior, _playerInput);

            foreach(Garage garage in testLevel.Garages.ToArray())
            {
                garage.Init(_timer);
            }

            _carSwitcher = new CarSwitcher(car,testLevel.Garages.ToArray(),_timer, _wheelPrefab);

            car.InitCar(_startConfig, _wheelPrefab);

            car.transform.position = testLevel.CarStartPosition;

            _cameraFollow.SetTarget(car.transform);
            car.transform.position = testLevel.CarStartPosition;
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
        }
    }
}
