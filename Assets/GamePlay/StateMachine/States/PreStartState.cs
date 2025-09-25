using Common;
using Common.States;
using GamePlay.Cars.Scripts;
using GamePlay.Infrastructure;
using GamePlay.UI.Scripts;
using Infrastructure.DI;
using Services.InputService;
using UnityEngine;

namespace GamePlay.StateMachine.States
{
    public class PreStartState : State
    {
        private readonly CarBehaviour _carBehaviour;
        private readonly PlayerInput _playerInput;
        private readonly StartMessage _startMessage;
        private readonly PauseManager _pauseManager;
        private readonly PauseButton _pauseButton;
        public PreStartState(Common.States.StateMachine stateMachine, IDIContainer sceneContext) : base(stateMachine)
        {
            _carBehaviour = sceneContext.Get<Car>().CarBehavior;
            _playerInput = sceneContext.Get<PlayerInput>();
            _startMessage = sceneContext.Get<StartMessage>();
            _pauseManager = sceneContext.Get<PauseManager>();
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
            
            _pauseManager.Resume();
        }

        protected override void OnExit()
        {
            _playerInput.screenInput -= OnScreenInput;
            
            if (_startMessage != null)
                _startMessage.Hide();

            if (!_pauseManager.IsPaused.Value)
                _pauseButton.Show();
        }

        private void OnScreenInput(Vector2 position)
        {
            if (_pauseManager.IsPaused.Value)
                return;

            _stateMachine.ChangeState<RaceGameState>();
        }
    }
}
