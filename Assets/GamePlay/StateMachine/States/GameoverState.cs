using Common.States;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.States
{
    public class GameoverState : State
    {
        private readonly CarBehaviour _car;
        public GameoverState(StateMachine stateMachine, CarBehaviour carBehaviour) : base(stateMachine)
        {
            _car = carBehaviour;
        }

        public override void Enter()
        {
            Debug.Log("GAME OVER");

            _car.enabled = false;
        }
    }
}
