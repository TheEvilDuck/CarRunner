using Common.States;
using DI;
using Gameplay.Cars;
using Services.PlayerInput;
using UnityEngine;

namespace Gameplay.States
{
    public class PreStartState : State
    {
        private readonly CarBehaviour _carBehaviour;
        private readonly IPlayerInput _playerInput;
        public PreStartState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _carBehaviour = sceneContext.Get<Car>().CarBehavior;
            _playerInput = sceneContext.Get<IPlayerInput>();
        }

        protected override void OnEnter()
        {
            _carBehaviour.enabled = false;
            _playerInput.Enable();
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _stateMachine.ChangeState<RaceGameState>();
        }
    }
}
