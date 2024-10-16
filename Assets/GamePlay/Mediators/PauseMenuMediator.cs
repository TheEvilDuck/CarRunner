using System;
using Common;
using DI;
using Services.SceneManagement;
using UnityEngine;

namespace Gameplay
{
    public class PauseMenuMediator : IDisposable
    {
        private readonly SceneChangingButtons _pauseMenuButtons;
        private readonly PauseManager _pauseManager;
        private readonly ISceneManager _sceneManager;

        public PauseMenuMediator(DIContainer sceneContext)
        {
            _pauseMenuButtons = sceneContext.Get<SceneChangingButtons>();
            _pauseManager = sceneContext.Get<PauseManager>();
            _sceneManager = sceneContext.Get<ISceneManager>();

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
            _sceneManager.LoadScene(SceneIDs.GAMEPLAY);
        }

        private void OnExitPressed()
        {
            _pauseManager.Unlock();
            _sceneManager.LoadScene(SceneIDs.MAIN_MENU);
        }
    }
}