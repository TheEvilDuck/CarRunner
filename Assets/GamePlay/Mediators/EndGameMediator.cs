using System;
using Common;
using Common.CoroutinePerformer;
using GamePlay.UI.Scripts;
using Infrastructure.DI;
using Services.SceneManagement;

namespace GamePlay.Mediators
{
    public class EndGameMediator : IDisposable
    {
        private readonly EndOfTheGame _endGameUI;
        private readonly PauseManager _pauseManager;
        private readonly ISceneManager _sceneManager;
        private readonly ICoroutinePerformer _coroutinePerformer;

        public EndGameMediator(IDIContainer sceneContext)
        {
            _endGameUI = sceneContext.Get<EndOfTheGame>();
            _pauseManager = sceneContext.Get<PauseManager>();
            _sceneManager = sceneContext.Get<ISceneManager>();
            _coroutinePerformer = sceneContext.Get<ICoroutinePerformer>();

            _endGameUI.SceneChangingButtons.RestartButtonPressed.AddListener(RestartLevel);
            _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.AddListener(LoadMainMenu);
        }

        public void Dispose()
        {
            _endGameUI.SceneChangingButtons.RestartButtonPressed.RemoveListener(RestartLevel);
            _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.RemoveListener(LoadMainMenu);
        }

        private void RestartLevel()
        {
            _pauseManager.Unlock();
            _coroutinePerformer.StartCoroutine(_sceneManager.LoadScene(SceneIDs.GAMEPLAY));
        }

        private void LoadMainMenu()
        {
            _pauseManager.Unlock();
            _coroutinePerformer.StartCoroutine(_sceneManager.LoadScene(SceneIDs.MAIN_MENU));
        }
    }
}