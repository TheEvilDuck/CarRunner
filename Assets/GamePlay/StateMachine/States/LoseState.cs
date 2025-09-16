using GamePlay.UI.Scripts;
using Infrastructure.DI;

namespace GamePlay.StateMachine.States
{
    public class LoseState : GameOverState
    {
        public LoseState(Common.States.StateMachine stateMachine, IDIContainer sceneContext) : base(stateMachine, sceneContext)
        {
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _sceneContext.Get<EndOfTheGame>().Lose();
        }
    }
}
