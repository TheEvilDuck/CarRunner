using System;
using Common.Data;
using DI;

namespace Common.Mediators
{
    public class CoinsLeaderboardMediator : IDisposable
    {
        private readonly ILeaderBoardData _leaderBoardData;
        private readonly IPlayerData _playerData;

        public CoinsLeaderboardMediator(DIContainer context)
        {
            _leaderBoardData = context.Get<ILeaderBoardData>();
            _playerData = context.Get<IPlayerData>();

            _playerData.coinsChanged += OnCoinsChanged;
        }

        public void Dispose() => _playerData.coinsChanged -= OnCoinsChanged;

        private void OnCoinsChanged(int coins) => _leaderBoardData.SaveCoins(coins);
    }
}
