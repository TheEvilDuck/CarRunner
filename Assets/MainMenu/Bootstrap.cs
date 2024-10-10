using System;
using System.Collections.Generic;
using Common;
using Common.Data;
using Common.Disposables;
using Common.Mediators;
using Common.Sound;
using EntryPoint;
using Levels;
using MainMenu.LanguageSelection;
using MainMenu.Shop;
using Services.Localization;
using UnityEngine;

namespace MainMenu
{
    public class Bootstrap : MonoBehaviourBootstrap
    {
        public const string MAIN_MENU_DISPOSABLES_TAG = nameof(MAIN_MENU_DISPOSABLES_TAG);
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private NotEnoughMoneyPopup _notEnoughMoneyPopup;
        [SerializeField] private CoinsView _coinsView;
        [SerializeField] private ShopItemFactory _shopItemFactory;
        private ISettings _gameSettings;
        private void Start() 
        {
            _mainMenuView.SettingsMenu.Init(_sceneContext.Get<ICameraSettings>(), _sceneContext.Get<ISoundSettings>());
            var settingsAndSoundMediator = new SettingsAndSoundMediator(_sceneContext);
            _sceneContext.Get<CompositeDisposable>(MAIN_MENU_DISPOSABLES_TAG).Add(settingsAndSoundMediator);
            _sceneContext.Get<SoundController>().Play(SoundID.MainMenuMusic);
        }

        protected override void OnBeforeSceneChanged()
        {
            base.OnBeforeSceneChanged();

            _gameSettings.SaveSettings();
            _sceneContext.Get<CompositeDisposable>(MAIN_MENU_DISPOSABLES_TAG).Dispose();
        }

        protected override void Setup()
        {
            _sceneContext.Register(() => new CompositeDisposable(), MAIN_MENU_DISPOSABLES_TAG);
            _sceneContext.Register(_mainMenuView);
            _sceneContext.Register(_mainMenuView.SettingsMenu);
            _sceneContext.Register(_mainMenuView.LevelSelector);
            _sceneContext.Register(_mainMenuView.ShopView);
            _sceneContext.Register(_mainMenuView.TutorialView);
            _sceneContext.Register(_notEnoughMoneyPopup);
            _sceneContext.Register(_coinsView);
            _sceneContext.Register(_shopItemFactory);
            _sceneContext.Register(SetupLanguageSelectionUI);

            _gameSettings = _sceneContext.Get<ISettings>();

            var mainMenuMediator = new MainMenuMediator(_sceneContext);
            var settingsMediator = new SettingsAndUIMediator(_sceneContext);
            var coinsMediator = new CoinsMediator(_sceneContext);
            var tutorialMediator = new TutorialMediator(_sceneContext);
            var languageMediator = new LanguageMediator(_sceneContext);
            var disposables = _sceneContext.Get<CompositeDisposable>(MAIN_MENU_DISPOSABLES_TAG);

            disposables.Add(mainMenuMediator);
            disposables.Add(settingsMediator);
            disposables.Add(coinsMediator);
            disposables.Add(tutorialMediator);
            disposables.Add(languageMediator);

            IPlayerData playerData = _sceneContext.Get<IPlayerData>();
            DeviceType deviceType = _sceneContext.Get<DeviceType>();

            _mainMenuView.Init();
            _mainMenuView.LevelSelector.Init(
                playerData.PassedLevels, 
                playerData.AvailableLevels, 
                _sceneContext.Get<ILeaderBoardData>(), 
                _sceneContext.Get<LevelsDatabase>().TutorialLevelId
                );
            
            disposables.Add(_mainMenuView.LevelSelector);
            _mainMenuView.ShopView.Init(_shopItemFactory, _sceneContext);
            _mainMenuView.TutorialView.Init(deviceType);
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