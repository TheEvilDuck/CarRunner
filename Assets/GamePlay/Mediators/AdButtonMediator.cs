using System;
using System.Collections;
using Common;
using Common.CoroutinePerformer;
using GamePlay.Infrastructure;
using GamePlay.UI.Scripts;
using Infrastructure.DI;
using Levels.Scripts;
using Services.Ads;
using Services.PlayerData;
using Services.PlayerData.Rewards;

namespace GamePlay.Mediators
{
    public class AdButtonMediator : IDisposable
    {
        private readonly AdButton _adButton;
        private readonly IAdsService _adsService;
        private readonly EndOfTheGame _endOfTheGame;
        private readonly ICoroutinePerformer _coroutinePerformer;
        private readonly RewardProvider _rewardProvider;
        private readonly Timer.Timer _timer;
        private readonly LevelsDatabase _levelsDatabase;
        private readonly IPlayerData _playerData;

        public AdButtonMediator(IDIContainer sceneContext)
        {
            _adButton = sceneContext.Get<EndOfTheGame>().AdButton;
            _adsService = sceneContext.Get<IAdsService>();
            _endOfTheGame = sceneContext.Get<EndOfTheGame>();
            _rewardProvider = sceneContext.Get<RewardProvider>();
            _timer = sceneContext.Get<Timer.Timer>();
            _levelsDatabase = sceneContext.Get<LevelsDatabase>();
            _playerData = sceneContext.Get<IPlayerData>();

            _adButton.clicked.AddListener(OnAdButtonPressed);
        }
        public void Dispose()
        {
            _adButton.clicked.RemoveListener(OnAdButtonPressed);
        }

        private void OnAdButtonPressed()
            => _coroutinePerformer.StartCoroutine(OnAdButtonPressedAsync());

        private IEnumerator OnAdButtonPressedAsync()
        {
            _endOfTheGame.Hide();

            void OnAdWatched(bool success)
            {
                int rewardCoins 
                    = _rewardProvider.GetLevelCompletionReward(_timer.CurrentTime, _playerData, _levelsDatabase);
                
                _playerData.AddCoins(rewardCoins);
                _endOfTheGame.Win(rewardCoins * 2);
                _adButton.Hide();
            }

            yield return _adsService.ShowRewardedAd(Bootstrap.WATCH_AD_REWAD_ID, OnAdWatched);

            _endOfTheGame.Show();
        }
    }
}