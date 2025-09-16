using System;
using Infrastructure.DI;
using MainMenu.Tutorial.Scripts;
using Services.PlayerData;
using Services.PlayerData.Rewards;

namespace MainMenu.Mediators
{
    public class TutorialMediator : IDisposable
    {
        private TutorialView _tutorialView;
        private IPlayerData _playerData;
        private RewardProvider _rewardProvider;

        public TutorialMediator(IDIContainer sceneContext)
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