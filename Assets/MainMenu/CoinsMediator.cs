using System;
using Common.Data;
using DI;

namespace MainMenu
{
    public class CoinsMediator : IDisposable
    {
        private readonly DIContainer _sceneContext;
        private readonly IPlayerData _playerData;
        private readonly CoinsView _coinsView;

        public CoinsMediator(DIContainer sceneContext)
        {
            _sceneContext = sceneContext;
            _playerData = _sceneContext.Get<IPlayerData>();
            _coinsView = _sceneContext.Get<CoinsView>();

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
