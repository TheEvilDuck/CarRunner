using Common.States;
using Gameplay.States;
using UnityEngine;

namespace Gameplay.CarFallingHandling
{
    public class FallingEndGame : ICarFallingHandler
    {
        private readonly StateMachine _gamePLayStateMachine;

        public FallingEndGame(StateMachine gamePlayerStateMachine)
        {
            _gamePLayStateMachine = gamePlayerStateMachine;
        }
        public void HandleFalling(Vector3 lastCarPosition, Quaternion lastCarRotation)
        {
            _gamePLayStateMachine.ChangeState<LoseState>();
        }
    }
}
