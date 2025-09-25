using System;
using System.Collections;
using Common;
using GamePlay.UI.Scripts;
using UnityEngine;
using YG;

namespace Services.Integrations
{
    public class YandexGameIntegrator: IDisposable
    {
        private readonly PauseManager _pauseManager;
        
        private bool _isInGameplay;

        public YandexGameIntegrator(PauseManager pauseManager)
        {
            _pauseManager = pauseManager;

            YandexGame.GetDataEvent += OnYandexGameGetDataEvent;
            _pauseManager.IsPaused.changed += OnPauseChanged;
        }

        public bool IsInitialized { get; private set; }

        public IEnumerator Initialize()
        {
            yield return new WaitUntil(() => IsInitialized);
        }
        
        public void MarkGameReady() => YandexGame.GameReadyAPI();

        public void MarkGameplay(bool isInGameplay)
        {
            _isInGameplay = isInGameplay;
            UpdateGameplayStatus(_pauseManager.IsPaused.Value);
        }
        
        public void Dispose()
        {
            YandexGame.GetDataEvent -= OnYandexGameGetDataEvent;
            _pauseManager.IsPaused.changed -= OnPauseChanged;
        }

        private void OnYandexGameGetDataEvent()
        {
            YandexGame.GetDataEvent -= OnYandexGameGetDataEvent;
            IsInitialized = true;
        }

        private void OnPauseChanged(bool paused) => UpdateGameplayStatus(paused);

        private void UpdateGameplayStatus(bool isInPause)
        {
            if (_isInGameplay == false)
            {
                YandexGame.GameplayStop();
                return;
            }
            
            if (isInPause)
                YandexGame.GameplayStop();
            else
                YandexGame.GameplayStart();
        }
    }
}