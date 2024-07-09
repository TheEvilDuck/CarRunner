using Common.States;
using DI;

namespace Gameplay.States
{
    public class LoseState : GameOverState
    {
        public LoseState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine, sceneContext)
        {
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _sceneContext.Get<EndOfTheGame>().Lose();
        }
    }
}
