using Common.States;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.States
{
    public class LoseState : GameOverState
    {
        public LoseState(StateMachine stateMachine, CarBehaviour car) : base(stateMachine, car)
        {
        }

        protected override void OnGameOver()
        {
            Debug.Log("LOSER HAHA SUCK A DICK");
        }
    }
}
