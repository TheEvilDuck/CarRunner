using System;
using System.Collections;
using System.Collections.Generic;
using Common.Data;
using Common.Data.Rewards;
using DI;
using Gameplay.UI;
using Levels;
using UnityEngine;

namespace Gameplay
{
    public class AdButtonMediator : IDisposable
    {
        private readonly DIContainer _sceneContext;
        private AdButton _adButton;
        public AdButtonMediator(DIContainer sceneContext)
        {
            _sceneContext = sceneContext;
            _adButton = _sceneContext.Get<EndOfTheGame>().AdButton;

            _adButton.clicked.AddListener(OnAdButtonPressed);
        }
        public void Dispose()
        {
            _adButton.clicked.RemoveListener(OnAdButtonPressed);
        }

        private void OnAdButtonPressed()
        {
            _sceneContext.Get<EndOfTheGame>().Hide();
            //здесь будем вызывать рекламу, времено вызову метод прямо тут
            OnAdWatched();
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