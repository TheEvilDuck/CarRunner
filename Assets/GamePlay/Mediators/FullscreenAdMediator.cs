using Common;
using DI;
using System;
using UnityEngine.UI;
using YG;

namespace Gameplay
{
    public class FullscreenAdMediator : IDisposable
    {
        private DIContainer _sceneContext;
        private PauseManager _pauseManager;
        private Image _anticlicker;

        public FullscreenAdMediator(DIContainer sceneContext)
        {
            _sceneContext = sceneContext;
            _pauseManager = _sceneContext.Get<PauseManager>();
            _anticlicker = _sceneContext.Get<Image>();

            YandexGame.OpenFullAdEvent += OnOpenFullAdEvent;
            YandexGame.CloseFullAdEvent += OnCloseFullAdEvent;
        }

        private void OnOpenFullAdEvent()
        {
            _anticlicker.gameObject.SetActive(true);
            _pauseManager.Pause();
        }

        private void OnCloseFullAdEvent()
        {
            _anticlicker.gameObject.SetActive(false);
            _pauseManager.Resume();
        }

        public void Dispose()
        {
            YandexGame.OpenFullAdEvent -= OnOpenFullAdEvent;
            YandexGame.CloseFullAdEvent -= OnCloseFullAdEvent;
        }
    }
}