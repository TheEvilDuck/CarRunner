using Common;
using Common.States;
using DI;
using Gameplay.Cars;
using Gameplay.UI;
using Services.PlayerInput;
using UnityEngine;

namespace Gameplay.States
{
    public class PreStartState : State
    {
        private readonly CarBehaviour _carBehaviour;
        private readonly IPlayerInput _playerInput;
        private readonly StartMessage _startMessage;
        private readonly PauseManager _globalPause;
        private readonly PauseButton _pauseButton;
        public PreStartState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _carBehaviour = sceneContext.Get<Car>().CarBehavior;
            _playerInput = sceneContext.Get<IPlayerInput>();
            _startMessage = sceneContext.Get<StartMessage>();
            _globalPause = sceneContext.Get<PauseManager>();
            _pauseButton = sceneContext.Get<PauseButton>();
        }

        public override void Update()
        {
            _carBehaviour.Brake(true);
        }

        protected override void OnEnter()
        {
            _carBehaviour.enabled = false;
            _playerInput.Enable();
            
            _startMessage.Show();

            _playerInput.screenInput += OnScreenInput;
            _pauseButton.Hide();
        }

        protected override void OnExit()
        {
            _playerInput.screenInput -= OnScreenInput;
            
            if (_startMessage != null)
                _startMessage.Hide();

            if (!_globalPause.IsPaused.Value)
                _pauseButton.Show();
        }

        private void OnScreenInput(Vector2 position)
        {
            if (_globalPause.IsPaused.Value)
                return;

            _stateMachine.ChangeState<RaceGameState>();
        }
    }
}
