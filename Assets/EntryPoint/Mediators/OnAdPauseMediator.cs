using System;
using Common;
using Infrastructure.DI;
using Services.Ads;

namespace EntryPoint.Mediators
{
    public class OnAdPauseMediator: IDisposable
    {
        private readonly PauseManager _pauseManager;
        private readonly IAdsService _adsService;

        public OnAdPauseMediator(IDIContainer container)
        {
            _pauseManager = container.Get<PauseManager>();
            _adsService = container.Get<IAdsService>();

            _adsService.adIsShown += OnAdShown;
            _adsService.adShowingStarted += OnAdShowingStarted;
        }

        public void Dispose()
        {
            _adsService.adIsShown -= OnAdShown;
            _adsService.adShowingStarted -= OnAdShowingStarted;
        }
        
        private void OnAdShowingStarted()
        {
            _pauseManager.Pause();
            _pauseManager.Lock();
        }

        private void OnAdShown()
        {
            _pauseManager.Unlock();
            _pauseManager.Resume();
        }
    }
}