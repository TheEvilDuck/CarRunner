using System;
using Infrastructure.DI;
using Services.Ads;
using Services.PlayerData;

namespace EntryPoint.Mediators
{
    public class OnAdSaveTimeMediator: IDisposable
    {
        private readonly IPlayerData _playerData;
        private readonly IAdsService _adsService;

        public OnAdSaveTimeMediator(IDIContainer container)
        {
            _playerData = container.Get<IPlayerData>();
            _adsService = container.Get<IAdsService>();

            _adsService.rewardAdIsShown += OnRewardAdIsShown;
        }
        
        public void Dispose() => _adsService.rewardAdIsShown -= OnRewardAdIsShown;
        private void OnRewardAdIsShown(int obj) => _playerData.SaveWatchAdLastTime();
    }
}