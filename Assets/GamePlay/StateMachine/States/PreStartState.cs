using Common.States;
using UnityEngine;

namespace Gameplay.States
{
    public class PreStartState : State
    {
        public PreStartState(StateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Update()
        {
            if (Input.GetMouseButtonDown(0))
                _stateMachine.ChangeState<RaceGameState>();
        }
    }
}
