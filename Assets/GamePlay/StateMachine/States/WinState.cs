using Common.CoroutinePerformer;
using GamePlay.UI.Scripts;
using Infrastructure.DI;
using Levels.Scripts;
using Services.LeaderBoards;
using Services.PlayerData;
using Services.RewardProvider;

namespace GamePlay.StateMachine.States
{
    public class WinState : GameOverState
    {
        public WinState(Common.States.StateMachine stateMachine, IDIContainer sceneContext) : base(stateMachine, sceneContext)
        {
        }

        protected override void OnEnter()
        {
            base.OnEnter();

            var playerData = _sceneContext.Get<IPlayerData>();
            var leaderboard = _sceneContext.Get<ILeaderBoardService>();
            var timer = _sceneContext.Get<Timer.Timer>();
            var rewardProvider = _sceneContext.Get<RewardProvider>();
            var levelsDatabase = _sceneContext.Get<LevelsDatabase>();
            var coroutinePerformer = _sceneContext.Get<ICoroutinePerformer>();

            playerData.AddPassedLevel(playerData.SelectedLevel);

            int coinsReward = rewardProvider.GetLevelCompletionReward(timer.CurrentTime, playerData.SelectedLevel);
            playerData.AddCoins(coinsReward);
            _sceneContext.Get<EndOfTheGame>().Win(coinsReward);

            if (timer.CurrentTime > 0 && !string.Equals(playerData.SelectedLevel, levelsDatabase.TutorialLevelId))
            {
                coroutinePerformer.StartCoroutine(leaderboard.SaveLevelRecordAsync(
                    playerData.SelectedLevel,
                    timer.CurrentTime));
            }
        }
    }
}
