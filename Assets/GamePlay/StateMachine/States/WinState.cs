using Common.States;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.States
{
    public class WinState : GameOverState
    {
        public WinState(StateMachine stateMachine, CarBehaviour car, CarControllerMediator carControllerMediator) : base(stateMachine, car, carControllerMediator)
        {
        }

        protected override void OnGameOver()
        {
            Debug.Log("WIN COOL WIN GREAT");
        }
    }
}
