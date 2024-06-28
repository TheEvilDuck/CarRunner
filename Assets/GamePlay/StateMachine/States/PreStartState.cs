using Common.States;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.States
{
    public class PreStartState : State
    {
        private readonly CarBehaviour _carBehaviour;
        public PreStartState(StateMachine stateMachine, CarBehaviour carBehaviour) : base(stateMachine)
        {
            _carBehaviour = carBehaviour;
        }

        protected override void OnEnter()
        {
            _carBehaviour.enabled = false;
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _stateMachine.ChangeState<RaceGameState>();
        }
    }
}
