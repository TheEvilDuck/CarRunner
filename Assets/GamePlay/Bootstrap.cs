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
        private Timer _timer;
        private List<IPausable>_pausableControls;
        private TimerMediator _timerMediator;
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
            RaceGameState raceGameState = new RaceGameState(_gameplayStateMachine, _timer);
            GameoverState gameoverState = new GameoverState(_gameplayStateMachine);

            _gameplayStateMachine.AddState(preStartState);
            _gameplayStateMachine.AddState(raceGameState);
            _gameplayStateMachine.AddState(gameoverState);
        }

        private void Update() 
        {
            _gameplayStateMachine.Update();
            _playerInput.Update();
        }

        private void OnDestroy() 
        {
            _timerMediator.Dispose();
        }
    }
}
