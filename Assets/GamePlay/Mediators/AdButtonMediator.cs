using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Data;
using Common.Data.Rewards;
using DI;
using Gameplay.UI;
using Levels;
using UnityEngine;
using YG;

namespace Gameplay
{
    public class AdButtonMediator : IDisposable
    {
        private readonly DIContainer _sceneContext;
        private AdButton _adButton;
        private IPlayerData _playerData;
        private PauseManager _pauseManager;
        public AdButtonMediator(DIContainer sceneContext)
        {
            _sceneContext = sceneContext;
            _adButton = _sceneContext.Get<EndOfTheGame>().AdButton;
            _playerData = sceneContext.Get<IPlayerData>();
            _pauseManager = sceneContext.Get<PauseManager>();

            _adButton.clicked.AddListener(OnAdButtonPressed);
        }
        public void Dispose()
        {
            _adButton.clicked.RemoveListener(OnAdButtonPressed);
        }

        private void OnAdButtonPressed()
        {
            _sceneContext.Get<EndOfTheGame>().Hide();
            
            Action<int> onRewardVideoEvent = null;

            onRewardVideoEvent = (int id) =>
            {
                if (id != Gameplay.Bootstrap.WATCH_AD_REWAD_ID)
                    return;
                
                _pauseManager.Resume();

                YandexGame.RewardVideoEvent -= onRewardVideoEvent;
                OnAdWatched();
            };

            YandexGame.RewardVideoEvent += onRewardVideoEvent;
            _pauseManager.Pause();
            YandexGame.RewVideoShow(Gameplay.Bootstrap.WATCH_AD_REWAD_ID);
        }

        private void OnAdWatched()
        {
            var rewardProvider = _sceneContext.Get<RewardProvider>();
            var timer = _sceneContext.Get<Timer>();
            var levelsDatabase = _sceneContext.Get<LevelsDatabase>();
            var playerData = _sceneContext.Get<IPlayerData>();
            var endOfTheGame = _sceneContext.Get<EndOfTheGame>();
            endOfTheGame.Show();

            int rewardCoins = rewardProvider.GetLevelCompletionReward(timer.CurrentTime, playerData, levelsDatabase);
            playerData.AddCoins(rewardCoins);
            endOfTheGame.Win(rewardCoins * 2);
            _adButton.Hide();
        }
    }

}