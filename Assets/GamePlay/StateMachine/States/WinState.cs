using Common;
using Common.Data;
using Common.States;
using DI;
using Levels;

namespace Gameplay.States
{
    public class WinState : GameOverState
    {
        public WinState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine, sceneContext)
        {
        }

        protected override void OnEnter()
        {
            base.OnEnter();
            _sceneContext.Get<EndOfTheGame>().Win();

            var playerData = _sceneContext.Get<IPlayerData>();
            playerData.AddPassedLevel(playerData.SelectedLevel);
        }
    }
}
