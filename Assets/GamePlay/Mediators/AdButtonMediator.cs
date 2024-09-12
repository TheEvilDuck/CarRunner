using System;
using Common;
using Common.Data;
using Common.Data.Rewards;
using DI;
using Gameplay.UI;
using Levels;
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

            void OnRewardWatched(int id)
            {
                OnRewardVideoClosed();

                if (id != Gameplay.Bootstrap.WATCH_AD_REWAD_ID)
                    return;

                YandexGame.RewardVideoEvent -= OnRewardWatched;
                OnAdWatched();
            }

            void OnRewardVideoClosed()
            {
                _pauseManager.Unlock();
                _pauseManager.Resume();
                YandexGame.ErrorVideoEvent -= OnRewardVideoClosed;
                YandexGame.CloseVideoEvent -= OnRewardVideoClosed;
                var endOfTheGame = _sceneContext.Get<EndOfTheGame>();
                endOfTheGame.Show();
            }

            YandexGame.RewardVideoEvent += OnRewardWatched;
            YandexGame.ErrorVideoEvent += OnRewardVideoClosed;
            YandexGame.CloseVideoEvent += OnRewardVideoClosed;

            _pauseManager.Pause();
            _pauseManager.Lock();

            YandexGame.RewVideoShow(Gameplay.Bootstrap.WATCH_AD_REWAD_ID);
        }

        private void OnAdWatched()
        {
            var rewardProvider = _sceneContext.Get<RewardProvider>();
            var timer = _sceneContext.Get<Timer>();
            var levelsDatabase = _sceneContext.Get<LevelsDatabase>();
            var playerData = _sceneContext.Get<IPlayerData>();
            var endOfTheGame = _sceneContext.Get<EndOfTheGame>();

            int rewardCoins = rewardProvider.GetLevelCompletionReward(timer.CurrentTime, playerData, levelsDatabase);
            playerData.AddCoins(rewardCoins);
            endOfTheGame.Win(rewardCoins * 2);
            _adButton.Hide();
        }
    }
}