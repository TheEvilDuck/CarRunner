using Common;
using Common.Data;
using DI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuMediator : IDisposable
    {
        private readonly MainMenuView _mainMenuView;
        private readonly IPlayerData _playerData;

        public MainMenuMediator(DIContainer sceneContainer)
        {
            _mainMenuView = sceneContainer.Get<MainMenuView>();
            _playerData = sceneContainer.Get<IPlayerData>();

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
            SceneManager.LoadScene(SceneIDs.GAMEPLAY);
        }
    }
}

