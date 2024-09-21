using Common;
using DI;
using Services.SceneManagement;
using System;

namespace Gameplay
{
    public class EndGameMediator : IDisposable
    {
        private readonly EndOfTheGame _endGameUI;
        private readonly PauseManager _pauseManager;
        private readonly ISceneManager _sceneManager;
        private readonly YandexGameFullScreenAd _yandexGameFullScreenAd;

        public EndGameMediator(DIContainer sceneContext)
        {
            _endGameUI = sceneContext.Get<EndOfTheGame>();
            _pauseManager = sceneContext.Get<PauseManager>();
            _sceneManager = sceneContext.Get<ISceneManager>();
            _yandexGameFullScreenAd = sceneContext.Get<YandexGameFullScreenAd>();

            _endGameUI.SceneChangingButtons.RestartButtonPressed.AddListener(RestartLevel);
            _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.AddListener(LoadMainMenu);
            _yandexGameFullScreenAd.AdIsShown += OnAdIsShown;
        }

        public void Dispose()
        {
            _endGameUI.SceneChangingButtons.RestartButtonPressed.RemoveListener(RestartLevel);
            _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.RemoveListener(LoadMainMenu);
            _yandexGameFullScreenAd.AdIsShown -= OnAdIsShown;
        }

        private void RestartLevel()
        {
            //_pauseManager.Unlock();
            //_sceneManager.LoadScene(SceneIDs.GAMEPLAY);
            _yandexGameFullScreenAd.ShowFullscreenAd();
        }

        private void LoadMainMenu()
        {
            _pauseManager.Unlock();
            _sceneManager.LoadScene(SceneIDs.MAIN_MENU);
        }

        private void OnAdIsShown()
        {
            _pauseManager.Unlock();
            _sceneManager.LoadScene(SceneIDs.GAMEPLAY);
        }
    }
}