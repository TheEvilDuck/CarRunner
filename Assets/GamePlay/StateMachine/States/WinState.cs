using Common.States;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.States
{
    public class WinState : State
    {
        private readonly CarBehaviour _car;
        public WinState(StateMachine stateMachine, CarBehaviour carBehaviour) : base(stateMachine)
        {
            _car = carBehaviour;
        }

        protected override void OnEnter()
        {
            Debug.Log("Win");

            _car.Stop();
        }
    }
}
