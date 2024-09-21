using System;
using Common;
using DI;
using Services.SceneManagement;

namespace Gameplay
{
    public class PauseMenuMediator : IDisposable
    {
        private readonly SceneChangingButtons _pauseMenuButtons;
        private readonly PauseManager _pauseManager;
        private readonly ISceneManager _sceneManager;
        private readonly YandexGameFullScreenAd _yandexGameFullScreenAd;

        public PauseMenuMediator(DIContainer sceneContext)
        {
            _pauseMenuButtons = sceneContext.Get<SceneChangingButtons>();
            _pauseManager = sceneContext.Get<PauseManager>();
            _sceneManager = sceneContext.Get<ISceneManager>();
            _yandexGameFullScreenAd = sceneContext.Get<YandexGameFullScreenAd>();

            _pauseMenuButtons.RestartButtonPressed.AddListener(OnRestartPressed);
            _pauseMenuButtons.GoToMainMenuButtonPressed.AddListener(OnExitPressed);

            _yandexGameFullScreenAd.AdIsShown += OnAdIsShown;
        }
        public void Dispose()
        {
            _pauseMenuButtons.RestartButtonPressed.RemoveListener(OnRestartPressed);
            _pauseMenuButtons.GoToMainMenuButtonPressed.RemoveListener(OnExitPressed);

            _yandexGameFullScreenAd.AdIsShown -= OnAdIsShown;
        }

        private void OnRestartPressed()
        {
            //_pauseManager.Unlock();
            //_sceneManager.LoadScene(SceneIDs.GAMEPLAY);
            _yandexGameFullScreenAd.ShowFullscreenAd();
        }

        private void OnExitPressed()
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