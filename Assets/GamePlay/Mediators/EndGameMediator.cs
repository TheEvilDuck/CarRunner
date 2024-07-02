using Common;
using Common.States;
using Gameplay.Cars;
using Gameplay.UI;
using Levels;
using System;

public class EndGameMediator : IDisposable
{
    private readonly EndOfTheGame _endGameUI;
    private readonly SceneLoader _sceneLoader;
    private readonly State _winState;
    private readonly State _gameOverState;
    private readonly PauseButton _pauseButton;
    private readonly CarControllerMediator _carControllerMediator;
    private readonly LevelsDatabase _levelDatabase;
    private readonly PlayerData _playerData;

    public EndGameMediator(EndOfTheGame endGameUI, SceneLoader sceneLoader, State gameOverState, State winState, PauseButton pauseButton, CarControllerMediator carControllerMediator, LevelsDatabase levelDatabase, PlayerData playerData)
    {
        _endGameUI = endGameUI;
        _sceneLoader = sceneLoader;
        _winState = winState;
        _gameOverState = gameOverState;
        _pauseButton = pauseButton;
        _carControllerMediator = carControllerMediator;
        _levelDatabase = levelDatabase;
        _playerData = playerData;

        _winState.entered += OnWinStateEntered;
        _gameOverState.entered += OnGameOverStateEntered;

        _endGameUI.SceneChangingButtons.RestartButtonPressed.AddListener(RestartLevel);
        _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.AddListener(LoadMainMenu);
    }

    private void OnWinStateEntered() => OnGameEnd(true);

    private void OnGameOverStateEntered() => OnGameEnd(false);

    private void OnGameEnd(bool win)
    {
        _carControllerMediator.Dispose();

        if (win)
        {
            _endGameUI.Win();

            string nextLevelId = _levelDatabase.GetNextLevelId(_playerData.SelectedLevel);
            if (nextLevelId != _playerData.SelectedLevel)
                _playerData.AddAvailableLevel(nextLevelId);
            _playerData.AddPassedLevel(_playerData.SelectedLevel);
        }
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

        _endGameUI.SceneChangingButtons.RestartButtonPressed.RemoveListener(RestartLevel);
        _endGameUI.SceneChangingButtons.GoToMainMenuButtonPressed.RemoveListener(LoadMainMenu);
    }
}
