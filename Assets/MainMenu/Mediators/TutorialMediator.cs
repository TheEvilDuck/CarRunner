using Common.Data;
using Common.Data.Rewards;
using DI;
using System;

namespace MainMenu
{
    public class TutorialMediator : IDisposable
    {
        private TutorialView _tutorialView;
        private IPlayerData _playerData;
        private RewardProvider _rewardProvider;

        public TutorialMediator(DIContainer sceneContext)
        {
            _tutorialView = sceneContext.Get<TutorialView>();
            _playerData = sceneContext.Get<IPlayerData>();
            _rewardProvider = sceneContext.Get<RewardProvider>();

            _tutorialView.UnderstandablePressed.AddListener(OnUnderstandablePressed);
        }

        public void Dispose() => _tutorialView.UnderstandablePressed.RemoveListener(OnUnderstandablePressed);

        private void OnUnderstandablePressed()
        {
            if(_playerData.IsTutorialComplete == false)
            {
                int rewardCoins = _rewardProvider.GetTutorialCompletionReward();
                _playerData.AddCoins(rewardCoins);
                _playerData.TutorialCmplete();
            }
        }
    }
}