using Common;
using DI;
using Gameplay.UI;
using System;
using UnityEngine.SceneManagement;

public class EndGameMediator : IDisposable
{
    private readonly EndOfTheGame _endGameUI;
    private readonly PauseButton _pauseButton;

    public EndGameMediator(DIContainer sceneContext)
    {
        _endGameUI = sceneContext.Get<EndOfTheGame>();

        _endGameUI.SceneChangingButtons.RestartButtonPressed.AddListener(RestartLevel);
        _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.AddListener(LoadMainMenu);
    }

    private void RestartLevel() => SceneManager.LoadScene(SceneIDs.GAMEPLAY);
    private void LoadMainMenu() => SceneManager.LoadScene(SceneIDs.MAIN_MENU);

    public void Dispose()
    {
        _endGameUI.SceneChangingButtons.RestartButtonPressed.RemoveListener(RestartLevel);
        _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.RemoveListener(LoadMainMenu);
    }
}
