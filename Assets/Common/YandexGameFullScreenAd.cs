using System;
using UnityEngine.Events;
using YG;

namespace Common
{
    public class YandexGameFullScreenAd : IDisposable
    {
        public event Action AdIsShown;

        public YandexGameFullScreenAd() => YandexGame.CloseFullAdEvent += OnCloseFullAdEvent;

        public void ShowFullscreenAd()
        {
            if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
                YandexGame.FullscreenShow();
            else
                AdIsShown?.Invoke();
        }

        public void Dispose() => YandexGame.CloseFullAdEvent -= OnCloseFullAdEvent;

        private void OnCloseFullAdEvent() => AdIsShown?.Invoke();
    }
}