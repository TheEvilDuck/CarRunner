using System;
using System.Collections.Generic;
using Common;
using Common.Data;
using Common.Mediators;
using Common.Sound;
using EntryPoint;
using MainMenu.LanguageSelection;
using MainMenu.Shop;
using Services.Localization;
using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviourBootstrap
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private NotEnoughMoneyPopup _notEnoughMoneyPopup;
        [SerializeField] private CoinsView _coinsView;
        [SerializeField] private ShopItemFactory _shopItemFactory;
        private GameSettings _gameSettings;
        private List<IDisposable> _disposables;
        private void Start() 
        {
            _mainMenuView.SettingsMenu.Init(_sceneContext.Get<ICameraSettings>(), _sceneContext.Get<ISoundSettings>());
            var settingsAndSoundMediator = new SettingsAndSoundMediator(_sceneContext);
            _disposables.Add(settingsAndSoundMediator);
            _sceneContext.Get<SoundController>().Play(SoundID.MainMenuMusic);
        }

        protected override void OnBeforeSceneChanged()
        {
            base.OnBeforeSceneChanged();

            _gameSettings.SaveSettings();
            
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();

            _disposables.Clear();
        }

        protected override void Setup()
        {
            _disposables = new List<IDisposable>();

            _sceneContext.Register(_disposables);
            _sceneContext.Register(_mainMenuView);
            _sceneContext.Register(_mainMenuView.SettingsMenu);
            _sceneContext.Register(_mainMenuView.LevelSelector);
            _sceneContext.Register(_mainMenuView.ShopView);
            _sceneContext.Register(_mainMenuView.TutorialView);
            _sceneContext.Register(_notEnoughMoneyPopup);
            _sceneContext.Register(_coinsView);
            _sceneContext.Register(_shopItemFactory);
            _sceneContext.Register(SetupLanguageSelectionUI);

            _gameSettings = _sceneContext.Get<GameSettings>();

            var mainMenuMediator = new MainMenuMediator(_sceneContext);
            var settingsMediator = new SettingsAndUIMediator(_sceneContext);
            var coinsMediator = new CoinsMediator(_sceneContext);
            var tutorialMediator = new TutorialMediator(_sceneContext);
            var languageMediator = new LanguageMediator(_sceneContext);

            _disposables.Add(mainMenuMediator);
            _disposables.Add(settingsMediator);
            _disposables.Add(coinsMediator);
            _disposables.Add(tutorialMediator);
            _disposables.Add(languageMediator);

            IPlayerData playerData = _sceneContext.Get<IPlayerData>();
            DeviceType deviceType = _sceneContext.Get<DeviceType>();

            _mainMenuView.Init();
            _mainMenuView.LevelSelector.Init(playerData.PassedLevels, playerData.AvailableLevels, _sceneContext.Get<ILeaderBoardData>());
            _disposables.Add(_mainMenuView.LevelSelector);
            _mainMenuView.ShopView.Init(_shopItemFactory, _sceneContext);
            _mainMenuView.TutorialView.Init(deviceType);
            Debug.Log(_sceneContext.Get<string>(EntryPoint.Bootstrap.PLATFORM_DI_TAG));
            //TODO заменить на сравнение с нужной платформой, я просто хз, какая стринга, в документации нет
            _mainMenuView.MainButtons.Init(true);

            if (Application.isFocused)
                _sceneContext.Get<PauseManager>().Resume();
            
        }

        private LanguageSelectorMenu SetupLanguageSelectionUI()
        {
            string currentLanguage = _sceneContext.Get<IPlayerData>().SavedPreferdLanguage.Value;

            if (string.IsNullOrEmpty(currentLanguage))
            {
                Debug.Log($"No saved language found, trying to get default language");
                currentLanguage = _sceneContext.Get<ILocalizationService>().CurrentLanguage;
            }

            _mainMenuView.LanguageSelectorMenu.Init(_sceneContext.Get<LanguageData[]>(), currentLanguage);
            return _mainMenuView.LanguageSelectorMenu;
        }
    }
}