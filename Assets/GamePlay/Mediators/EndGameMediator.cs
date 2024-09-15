using Common;
using DI;
using Services.SceneManagement;
using System;

public class EndGameMediator : IDisposable
{
    private readonly EndOfTheGame _endGameUI;
    private readonly PauseManager _pauseManager;
    private readonly ISceneManager _sceneManager;

    public EndGameMediator(DIContainer sceneContext)
    {
        _endGameUI = sceneContext.Get<EndOfTheGame>();
        _pauseManager = sceneContext.Get<PauseManager>();
        _sceneManager = sceneContext.Get<ISceneManager>();

        _endGameUI.SceneChangingButtons.RestartButtonPressed.AddListener(RestartLevel);
        _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.AddListener(LoadMainMenu);
    }

    private void RestartLevel()
    {
        _pauseManager.Unlock();
        _sceneManager.LoadScene(SceneIDs.GAMEPLAY);
    }
    private void LoadMainMenu()
    {
        _pauseManager.Unlock();
        _sceneManager.LoadScene(SceneIDs.MAIN_MENU);
    }

    public void Dispose()
    {
        _endGameUI.SceneChangingButtons.RestartButtonPressed.RemoveListener(RestartLevel);
        _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.RemoveListener(LoadMainMenu);
    }
}
