using Common.States;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.States
{
    public class LoseState : State
    {
        private readonly CarBehaviour _car;
        public LoseState(StateMachine stateMachine, CarBehaviour carBehaviour) : base(stateMachine)
        {
            _car = carBehaviour;
        }

        protected override void OnEnter()
        {
            Debug.Log("GAME OVER");

            _car.Stop();
        }
    }
}
