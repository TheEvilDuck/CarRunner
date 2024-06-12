using Common;
using Common.States;
using Gameplay.Cars;
using Gameplay.UI;
using System;

public class EndGameMediator : IDisposable
{
    private readonly EndOfTheGame _endGameUI;
    private readonly SceneLoader _sceneLoader;
    private readonly State _winState;
    private readonly State _gameOverState;
    private readonly PauseButton _pauseButton;
    private readonly CarControllerMediator _carControllerMediator;

    public EndGameMediator(EndOfTheGame endGameUI, SceneLoader sceneLoader, State gameOverState, State winState, PauseButton pauseButton, CarControllerMediator carControllerMediator)
    {
        _endGameUI = endGameUI;
        _sceneLoader = sceneLoader;
        _winState = winState;
        _gameOverState = gameOverState;
        _pauseButton = pauseButton;
        _carControllerMediator = carControllerMediator;

        _winState.entered += OnWinStateEntered;
        _gameOverState.entered += OnGameOverStateEntered;

        _endGameUI.RestartClickedEvent.AddListener(RestartLevel);
        _endGameUI.MainMenuClickedEvent.AddListener(LoadMainMenu);
    }

    private void OnWinStateEntered() => OnGameEnd(true);

    private void OnGameOverStateEntered() => OnGameEnd(false);

    private void OnGameEnd(bool win)
    {
        _carControllerMediator.Dispose();

        if (win)
            _endGameUI.Win();
        else
            _endGameUI.Lose();

        _endGameUI.Show();
        _pauseButton.Hide();
        
    }

    private void RestartLevel() => _sceneLoader.RestartScene();
    private void LoadMainMenu() => _sceneLoader.LoadMainMenu();

    public void Dispose()
    {
        _winState.entered -= OnWinStateEntered;
        _gameOverState.entered -= OnGameOverStateEntered;

        _endGameUI.RestartClickedEvent.RemoveListener(RestartLevel);
        _endGameUI.MainMenuClickedEvent.RemoveListener(LoadMainMenu);
    }
}
