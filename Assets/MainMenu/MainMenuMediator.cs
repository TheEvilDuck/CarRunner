using Common;
using System;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuMediator : IDisposable
    {
        private readonly MainMenuView _mainMenuView;
        private readonly PlayerData _playerData;
        private readonly SceneLoader _sceneLoader;

        public MainMenuMediator(MainMenuView mainMenuView, PlayerData playerData, SceneLoader sceneLoader)
        {
            _mainMenuView = mainMenuView;
            _sceneLoader = sceneLoader;
            _playerData = playerData;

            _mainMenuView.MainButtons.ExitClickedEvent.AddListener(OnExitPressed);
            _mainMenuView.LevelSelector.levelSelected += OnLevelSelected;
        }
        public void Dispose()
        {
            _mainMenuView.MainButtons.ExitClickedEvent.RemoveListener(OnExitPressed);
            _mainMenuView.LevelSelector.levelSelected -= OnLevelSelected;
        }

        private void OnExitPressed() => Application.Quit();
        private void OnLevelSelected(string levelId)
        {
            _playerData.SaveSelectedLevel(levelId);
            _sceneLoader.LoadGameplay();
        }
    }
}

