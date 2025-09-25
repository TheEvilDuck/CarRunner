using System;
using Common;
using Common.CoroutinePerformer;
using GamePlay.UI.Scripts;
using Infrastructure.DI;
using Services.SceneManagement;
using UnityEngine.SceneManagement;

namespace GamePlay.Mediators
{
    public class PauseMenuMediator : IDisposable
    {
        private readonly SceneChangingButtons _pauseMenuButtons;
        private readonly PauseManager _pauseManager;
        private readonly ISceneManager _sceneManager;
        private readonly ICoroutinePerformer _coroutinePerformer;

        public PauseMenuMediator(IDIContainer sceneContext)
        {
            _pauseMenuButtons = sceneContext.Get<SceneChangingButtons>();
            _pauseManager = sceneContext.Get<PauseManager>();
            _sceneManager = sceneContext.Get<ISceneManager>();
            _coroutinePerformer = sceneContext.Get<ICoroutinePerformer>();

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
            _coroutinePerformer.StartCoroutine(_sceneManager.LoadScene(SceneIDs.GAMEPLAY));
        }

        private void OnExitPressed()
        {
            _pauseManager.Unlock();
            _coroutinePerformer.StartCoroutine(_sceneManager.LoadScene(SceneIDs.MAIN_MENU));
        }
    }
}