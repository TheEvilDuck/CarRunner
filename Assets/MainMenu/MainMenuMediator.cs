using Common;
using Common.Data;
using DI;
using Levels;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuMediator : IDisposable
    {
        private readonly MainMenuView _mainMenuView;
        private readonly IPlayerData _playerData;
        private readonly LevelsDatabase _levelsDatabase;
        private readonly NotEnoughMoneyPopup _notEnoughMoneyPopup;

        public MainMenuMediator(DIContainer sceneContainer)
        {
            _mainMenuView = sceneContainer.Get<MainMenuView>();
            _playerData = sceneContainer.Get<IPlayerData>();
            _levelsDatabase = sceneContainer.Get<LevelsDatabase>();
            _notEnoughMoneyPopup = sceneContainer.Get<NotEnoughMoneyPopup>();

            _mainMenuView.MainButtons.ExitClickedEvent.AddListener(OnExitPressed);
            _mainMenuView.LevelSelector.levelSelected += OnLevelSelected;
            _mainMenuView.LevelSelector.buyLevelPressed += OnBuyLevelButtonPressed;
        }
        public void Dispose()
        {
            _mainMenuView.MainButtons.ExitClickedEvent.RemoveListener(OnExitPressed);
            _mainMenuView.LevelSelector.levelSelected -= OnLevelSelected;
            _mainMenuView.LevelSelector.buyLevelPressed -= OnBuyLevelButtonPressed;
        }

        private void OnExitPressed() => Application.Quit();
        private void OnLevelSelected(string levelId)
        {
            _playerData.SaveSelectedLevel(levelId);
            SceneManager.LoadScene(SceneIDs.GAMEPLAY);
        }

        private bool OnBuyLevelButtonPressed(string levelId)
        {
            if (_playerData.SpendCoins(_levelsDatabase.GetLevelCost(levelId)))
            {
                _playerData.AddAvailableLevel(levelId);
                _mainMenuView.LevelSelector.UpdateButtons(_playerData.PassedLevels, _playerData.AvailableLevels);
                return true;
            }

            _notEnoughMoneyPopup.Show();
            return false;
        }
    }
}

