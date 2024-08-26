using Common;
using DI;
using Gameplay.UI;
using System;
using UnityEngine.SceneManagement;

public class EndGameMediator : IDisposable
{
    private readonly EndOfTheGame _endGameUI;
    private readonly PauseManager _pauseManager;

    public EndGameMediator(DIContainer sceneContext)
    {
        _endGameUI = sceneContext.Get<EndOfTheGame>();
        _pauseManager = sceneContext.Get<PauseManager>();

        _endGameUI.SceneChangingButtons.RestartButtonPressed.AddListener(RestartLevel);
        _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.AddListener(LoadMainMenu);
    }

    private void RestartLevel()
    {
        _pauseManager.Unlock();
        SceneManager.LoadScene(SceneIDs.GAMEPLAY);
    }
    private void LoadMainMenu()
    {
        _pauseManager.Unlock();
        SceneManager.LoadScene(SceneIDs.MAIN_MENU);
    }

    public void Dispose()
    {
        _endGameUI.SceneChangingButtons.RestartButtonPressed.RemoveListener(RestartLevel);
        _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.RemoveListener(LoadMainMenu);
    }
}
