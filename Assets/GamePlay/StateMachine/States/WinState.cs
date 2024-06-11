using Common.States;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.States
{
    public class WinState : GameOverState
    {
        public WinState(StateMachine stateMachine, CarBehaviour car) : base(stateMachine, car)
        {
        }

        protected override void OnGameOver()
        {
            Debug.Log("WIN COOL WIN GREAT");
        }
    }
}
