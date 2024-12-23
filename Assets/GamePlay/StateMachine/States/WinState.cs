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
            var leaderboard = _sceneContext.Get<ILeaderBoardData>();
            var timer = _sceneContext.Get<Timer>();
            var rewardProvider = _sceneContext.Get<RewardProvider>();
            var levelsDatabase = _sceneContext.Get<LevelsDatabase>();

            playerData.AddPassedLevel(playerData.SelectedLevel);

            int coinsReward = rewardProvider.GetLevelCompletionReward(timer.CurrentTime, playerData, levelsDatabase);
            playerData.AddCoins(coinsReward);
            _sceneContext.Get<EndOfTheGame>().Win(coinsReward);

            if (timer.CurrentTime > 0 && !string.Equals(playerData.SelectedLevel, levelsDatabase.TutorialLevelId))
            {
                leaderboard.SaveLevelRecord(playerData.SelectedLevel, timer.CurrentTime);
            }
        }
    }
}
