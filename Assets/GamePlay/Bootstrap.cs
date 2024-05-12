using System.Collections.Generic;
using System.Linq;
using Common;
using Common.States;
using Gameplay.Cars;
using Gameplay.Garages;
using Gameplay.Levels;
using Gameplay.States;
using Gameplay.TimerGates;
using Gameplay.UI;
using Services.PlayerInput;
using UnityEngine;

namespace Gameplay
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private float _startTime = 20f;
        [SerializeField]private TimerView _timerView;
        [SerializeField]private LevelData _testLevelPRefab;
        [SerializeField]private Car _car;
        [SerializeField]private CarConfig _startConfig;
        [SerializeField]private GameObject _wheelPrefab;
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

            LevelData testLevel = Instantiate(_testLevelPRefab);
            testLevel.transform.position = Vector3.zero;

            _timer = new Timer(_startTime);
            _pausableControls = new List<IPausable>
            {
                _timer
            };

            _timerMediator = new TimerMediator(_timer, _timerView);

            _gameplayStateMachine = new StateMachine();

            PreStartState preStartState = new PreStartState(_gameplayStateMachine);
            RaceGameState raceGameState = new RaceGameState(_gameplayStateMachine, _timer, _car.CarBehavior, testLevel.Finish);
            WinState winState = new WinState(_gameplayStateMachine, _car.CarBehavior);
            LoseState loseState = new LoseState(_gameplayStateMachine, _car.CarBehavior);


            _gameplayStateMachine.AddState(preStartState);
            _gameplayStateMachine.AddState(raceGameState);
            _gameplayStateMachine.AddState(winState);
            _gameplayStateMachine.AddState(loseState);

            _timerAndGatesMediator = new TimerAndGatesMediator(testLevel.TimerGates.ToArray(), _timer);

            _carControllerMediator = new CarControllerMediator(_car.CarBehavior, _playerInput);

            foreach(Garage garage in testLevel.Garages.ToArray())
            {
                garage.Init(_timer);
            }

            _carSwitcher = new CarSwitcher(_car,testLevel.Garages.ToArray(),_timer, _wheelPrefab);

            _car.InitCar(_startConfig, _wheelPrefab);

            _car.transform.position = testLevel.CarStartPosition;
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
