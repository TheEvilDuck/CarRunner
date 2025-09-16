using System;
using Common.CoroutinePerformer;
using Infrastructure.DI;
using Services.LeaderBoards;
using Services.PlayerData;

namespace Common.Mediators
{
    public class CoinsLeaderboardMediator : IDisposable
    {
        private readonly ILeaderBoardService _leaderBoardService;
        private readonly IPlayerData _playerData;
        private readonly ICoroutinePerformer _coroutinePerformer;

        public CoinsLeaderboardMediator(IDIContainer context)
        {
            _leaderBoardService = context.Get<ILeaderBoardService>();
            _playerData = context.Get<IPlayerData>();
            _coroutinePerformer = context.Get<ICoroutinePerformer>();

            _playerData.coinsChanged += OnCoinsChanged;
        }

        public void Dispose() => _playerData.coinsChanged -= OnCoinsChanged;

        private void OnCoinsChanged(int coins) 
            => _coroutinePerformer.StartCoroutine(_leaderBoardService.SaveCoinsRecordAsync(coins));
    }
}
