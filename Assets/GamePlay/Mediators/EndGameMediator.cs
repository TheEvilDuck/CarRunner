using Common;
using Gameplay.States;
using System;

public class EndGameMediator : IDisposable
{
    private EndOfTheGame _endGameUI;
    private SceneLoader _sceneLoader;
    private WinState _winState;
    private GameOverState _gameOverState;

    public EndGameMediator(EndOfTheGame endGameUI, SceneLoader sceneLoader, GameOverState gameOverState, WinState winState)
    {
        _endGameUI = endGameUI;
        _sceneLoader = sceneLoader;
        _winState = winState;
        _gameOverState = gameOverState;

        _winState.entered += onWinStateEntered;
        _gameOverState.entered += onGameOverStateEntered;

        _endGameUI.RestartClickedEvent.AddListener(restartLevel);
        _endGameUI.MainMenuClickedEvent.AddListener(loadMainMenu);
    }

    private void onWinStateEntered()
    {
        _endGameUI.Show();
        _endGameUI.Win();
    }

    private void onGameOverStateEntered()
    {
        _endGameUI.Show();
        _endGameUI.Lose();
    }

    private void restartLevel() => _sceneLoader.RestartScene();
    private void loadMainMenu() => _sceneLoader.LoadMainMenu();

    public void Dispose()
    {
        _winState.entered -= onWinStateEntered;
        _gameOverState.entered -= onGameOverStateEntered;
    }
}
