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
        public PreStartState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _carBehaviour = sceneContext.Get<Car>().CarBehavior;
            _playerInput = sceneContext.Get<IPlayerInput>();
            _startMessage = sceneContext.Get<StartMessage>();
        }

        protected override void OnEnter()
        {
            _carBehaviour.enabled = false;
            _playerInput.Enable();
            
            _startMessage.Show();

            _playerInput.screenInput += OnScreenInput;
        }

        protected override void OnExit()
        {
            _playerInput.screenInput -= OnScreenInput;
            _startMessage.Hide();
        }

        private void OnScreenInput(Vector2 position)
        {
            _stateMachine.ChangeState<RaceGameState>();
        }
    }
}
