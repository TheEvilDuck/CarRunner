using System;
using System.Collections;
using System.Collections.Generic;
using Services.TimerService;
using UnityEngine;
using YG;

namespace Services.Ads
{
    public class YandexGameFullScreenAd : IDisposable, IAdsService
    {
        public event Action adIsShown;
        public event Action adShowingStarted;
        public event Action<int> rewardAdIsShown;

        private readonly ITimerService _timerService;
        private readonly List<int> _rewardedAdsBuffer = new List<int>();

        private float _lastFullScreenAdTime;
        
        public bool Ready 
            => _timerService.CurrentTime - _lastFullScreenAdTime >= YandexGame.Instance.infoYG.fullscreenAdInterval;

        public YandexGameFullScreenAd(ITimerService timerService)
        {
            _timerService = timerService;
            
            YandexGame.CloseFullAdEvent += OnCloseFullAdEvent;
            YandexGame.RewardVideoEvent += OnRewardVideoEvent;
            YandexGame.ErrorFullAdEvent += OnFullAddError;
        }
        
        public IEnumerator ShowFullscreenAd()
        {
            if (Ready == false)
                yield break;
            
            adShowingStarted?.Invoke();
            YandexGame.FullscreenShow();
        }

        public IEnumerator ShowRewardedAd(int adId, Action<bool> callback)
        {
            adShowingStarted?.Invoke();
            YandexGame.RewVideoShow(adId);
            yield return new WaitUntil(() => IsVideoAdClosed(adId));
            adIsShown?.Invoke();
            callback?.Invoke(HandleVideoSuccess(adId));
        }

        public void Dispose()
        {
            YandexGame.CloseFullAdEvent -= OnCloseFullAdEvent;
            YandexGame.RewardVideoEvent -= OnRewardVideoEvent;
        }

        private void OnCloseFullAdEvent() => adIsShown?.Invoke();

        private void OnRewardVideoEvent(int rewardID)
        {
            rewardAdIsShown?.Invoke(rewardID);
            _rewardedAdsBuffer.Add(rewardID);
        }
        private void OnFullAddError() => adIsShown?.Invoke();

        private bool IsVideoAdClosed(int id)
            => _rewardedAdsBuffer.Contains(id);

        private bool HandleVideoSuccess(int id)
        {
            if (_rewardedAdsBuffer.Contains(id))
            {
                _rewardedAdsBuffer.Remove(id);
                return true;
            }
            
            return false;
        }
    }
}