using Common.Data;
using DI;
using System;

namespace MainMenu
{
    public class TutorialMediator : IDisposable
    {
        private TutorialView _tutorialView;
        private IPlayerData _playerData;
        private RewardForTutorial _rewardForTutorial;
        private DIContainer _sceneContext;

        public TutorialMediator(DIContainer sceneContext)
        {
            _tutorialView = sceneContext.Get<TutorialView>();
            _playerData = sceneContext.Get<IPlayerData>();
            _rewardForTutorial = sceneContext.Get<RewardForTutorial>();
            _sceneContext = sceneContext;

            _tutorialView.UnderstandablePressed.AddListener(OnUnderstandablePressed);
        }

        public void Dispose() => _tutorialView.UnderstandablePressed.RemoveListener(OnUnderstandablePressed);

        private void OnUnderstandablePressed()
        {
            if(_playerData.IsTutorialComplete == false)
            {
                _rewardForTutorial.TryClaim(_sceneContext);
                _playerData.TutorialCmplete();
            }
        }
    }
}