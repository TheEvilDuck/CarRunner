using System;
using Infrastructure.DI;
using Services.PlayerData;

namespace MainMenu.Mediators
{
    public class CoinsMediator : IDisposable
    {
        private readonly IPlayerData _playerData;
        private readonly CoinsView _coinsView;

        public CoinsMediator(IDIContainer container)
        {
            _playerData = container.Get<IPlayerData>();
            _coinsView = container.Get<CoinsView>();

            _playerData.coinsChanged += OnCoinsChanged;

            OnCoinsChanged(_playerData.Coins);
        }
        public void Dispose()
        {
            _playerData.coinsChanged -= OnCoinsChanged;
        }

        private void OnCoinsChanged(int value)
        {
            _coinsView.UpdateValue(value);
        }
    }
}
