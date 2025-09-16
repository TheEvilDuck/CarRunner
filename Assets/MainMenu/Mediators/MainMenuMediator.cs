using System;
using Infrastructure.DI;
using Levels.Scripts;
using Services.Ads;
using Services.PlayerData;
using Services.SceneManagement;
using UnityEngine;

namespace MainMenu.Mediators
{
    public class MainMenuMediator : IDisposable
    {
        private readonly MainMenuView _mainMenuView;
        private readonly IPlayerData _playerData;
        private readonly LevelsDatabase _levelsDatabase;
        private readonly NotEnoughMoneyPopup _notEnoughMoneyPopup;
        private readonly ISceneManager _sceneManager;
        private readonly YandexGameFullScreenAd _yandexGameFullScreenAd;

        public MainMenuMediator(IDIContainer sceneContainer)
        {
            _mainMenuView = sceneContainer.Get<MainMenuView>();
            _playerData = sceneContainer.Get<IPlayerData>();
            _levelsDatabase = sceneContainer.Get<LevelsDatabase>();
            _notEnoughMoneyPopup = sceneContainer.Get<NotEnoughMoneyPopup>();
            _sceneManager = sceneContainer.Get<ISceneManager>();
            _yandexGameFullScreenAd = sceneContainer.Get<YandexGameFullScreenAd>();

            _mainMenuView.MainButtons.ExitClickedEvent.AddListener(OnExitPressed);
            _mainMenuView.LevelSelector.levelSelected += OnLevelSelected;
            _mainMenuView.LevelSelector.buyLevelPressed += OnBuyLevelButtonPressed;
            _notEnoughMoneyPopup.yesClicked += OnYesNotEnoughMoneyPopupPressed;
            _yandexGameFullScreenAd.adIsShown += OnAdIsShown;
        }

        public void Dispose()
        {
            _mainMenuView.MainButtons.ExitClickedEvent.RemoveListener(OnExitPressed);
            _mainMenuView.LevelSelector.levelSelected -= OnLevelSelected;
            _mainMenuView.LevelSelector.buyLevelPressed -= OnBuyLevelButtonPressed;
            _notEnoughMoneyPopup.yesClicked -= OnYesNotEnoughMoneyPopupPressed;
            _yandexGameFullScreenAd.adIsShown -= OnAdIsShown;
        }

        private void OnExitPressed() => Application.Quit();

        private void OnAdIsShown() => _sceneManager.LoadScene(SceneIDs.GAMEPLAY);

        private void OnLevelSelected(string levelId)
        {
            _playerData.SaveSelectedLevel(levelId);
            _yandexGameFullScreenAd.ShowFullscreenAd();
        }

        private bool OnBuyLevelButtonPressed(string levelId)
        {
            if (_levelsDatabase.GetLevelCost(levelId) <= 0 || _playerData.SpendCoins(_levelsDatabase.GetLevelCost(levelId)))
            {
                _playerData.AddAvailableLevel(levelId);
                _mainMenuView.LevelSelector.UpdateButtons(_playerData.PassedLevels, _playerData.AvailableLevels);
                return true;
            }

            _notEnoughMoneyPopup.Show();
            return false;
        }

        private void OnYesNotEnoughMoneyPopupPressed() => _mainMenuView.ShowShopMenu();
    }
}

