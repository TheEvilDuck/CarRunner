using System;
using Common;
using DI;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class PauseMenuMediator : IDisposable
    {
        private readonly SceneChangingButtons _pauseMenuButtons;
        private readonly PauseManager _pauseManager;

        public PauseMenuMediator(DIContainer sceneContext)
        {
            _pauseMenuButtons = sceneContext.Get<SceneChangingButtons>();
            _pauseManager = sceneContext.Get<PauseManager>();

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
            _pauseManager.Unlock();
            SceneManager.LoadScene(SceneIDs.GAMEPLAY);
        }

        private void OnExitPressed()
        {
            _pauseManager.Unlock();
            SceneManager.LoadScene(SceneIDs.MAIN_MENU);
        }
    }
}
