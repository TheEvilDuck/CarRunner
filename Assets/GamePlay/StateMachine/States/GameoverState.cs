using Common.States;
using UnityEngine;

namespace Gameplay.States
{
    public class GameoverState : State
    {
        public GameoverState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            Debug.Log("GAME OVER");
        }
    }
}
