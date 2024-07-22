using Common.Data;
using Common.Data.Rewards;
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

            var playerData = _sceneContext.Get<IPlayerData>();
            var timer = _sceneContext.Get<Timer>();
            var rewardProvider = _sceneContext.Get<RewardProvider>();

            playerData.AddPassedLevel(playerData.SelectedLevel);

            int coinsReward = rewardProvider.GetLevelCompletionReward(timer.CurrentTime, playerData, _sceneContext.Get<LevelsDatabase>());
            playerData.AddCoins(coinsReward);
            _sceneContext.Get<EndOfTheGame>().Win(coinsReward);

            playerData.SaveLevelRecord(playerData.SelectedLevel, timer.CurrentTime);
        }
    }
}
