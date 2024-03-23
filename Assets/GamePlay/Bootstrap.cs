using System.Collections.Generic;
using Common;
using Common.States;
using Gameplay.States;
using Gameplay.UI;
using Services.PlayerInput;
using UnityEngine;

namespace Gameplay
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField]private float _startTime = 20f;
        [SerializeField]private TimerView _timerView;
        [SerializeField]private TimerGate[] _timerGates;
        [SerializeField]private Garage[] _garages;
        [SerializeField]private CarBehaviour _car;
        private Timer _timer;
        private List<IPausable>_pausableControls;
        private TimerMediator _timerMediator;
        private TimerAndGatesMediator _timerAndGatesMediator;
        private CarControllerMediator _carControllerMediator;
        private StateMachine _gameplayStateMachine;
        private IPlayerInput _playerInput;

        private void Awake() 
        {
            if (SystemInfo.deviceType== DeviceType.Desktop)
                _playerInput = new DesktopInput();
            else if (SystemInfo.deviceType == DeviceType.Handheld)
                _playerInput = new MobileInput();

            _timer = new Timer(_startTime);
            _pausableControls = new List<IPausable>();
            _pausableControls.Add(_timer);

            _timerMediator = new TimerMediator(_timer, _timerView);

            _gameplayStateMachine = new StateMachine();

            PreStartState preStartState = new PreStartState(_gameplayStateMachine);
            RaceGameState raceGameState = new RaceGameState(_gameplayStateMachine, _timer, _car);
            GameoverState gameoverState = new GameoverState(_gameplayStateMachine, _car);

            _gameplayStateMachine.AddState(preStartState);
            _gameplayStateMachine.AddState(raceGameState);
            _gameplayStateMachine.AddState(gameoverState);

            _timerAndGatesMediator = new TimerAndGatesMediator(_timerGates, _timer);

            _carControllerMediator = new CarControllerMediator(_car, _playerInput);

            foreach(Garage garage in _garages)
            {
                garage.Init(_timer);
            }
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
        }
    }
}
