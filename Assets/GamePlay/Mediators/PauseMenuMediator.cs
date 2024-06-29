using System;
using Common;

namespace Gameplay
{
    public class PauseMenuMediator : IDisposable
    {
        private readonly SceneChangingButtons _pauseMenuButtons;
        private readonly SceneLoader _sceneLoader;

        public PauseMenuMediator(SceneChangingButtons pauseMenuButtons, SceneLoader sceneLoader)
        {
            _pauseMenuButtons = pauseMenuButtons;
            _sceneLoader = sceneLoader;

            _pauseMenuButtons.RestartButtonPressed.AddListener(OnRestartPressed);
            _pauseMenuButtons.GoToMainMenuButtonPressed.AddListener(OnExitPressed);
        }
        public void Dispose()
        {
            _pauseMenuButtons.RestartButtonPressed.RemoveListener(OnRestartPressed);
            _pauseMenuButtons.GoToMainMenuButtonPressed.RemoveListener(OnExitPressed);
        }

        private void OnRestartPressed()
        {
            _sceneLoader.RestartScene();
        }

        private void OnExitPressed()
        {
            _sceneLoader.LoadMainMenu();
        }
    }
}
