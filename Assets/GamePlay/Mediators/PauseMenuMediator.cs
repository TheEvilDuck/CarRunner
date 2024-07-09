using System;
using Common;
using DI;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class PauseMenuMediator : IDisposable
    {
        private readonly SceneChangingButtons _pauseMenuButtons;

        public PauseMenuMediator(DIContainer sceneContext)
        {
            _pauseMenuButtons = sceneContext.Get<SceneChangingButtons>();

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
            SceneManager.LoadScene(SceneIDs.GAMEPLAY);
        }

        private void OnExitPressed()
        {
            SceneManager.LoadScene(SceneIDs.MAIN_MENU);
        }
    }
}
