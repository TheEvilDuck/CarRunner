using System.Collections;
using Common;
using Common.DeviceTypeHandling;
using Common.Disposables;
using Common.LoadingCurtain;
using Common.Mediators;
using Common.Settings;
using Common.Sounds.Scripts;
using Infrastructure.Bootstraps;
using Infrastructure.DI;
using Levels.Scripts;
using MainMenu.Mediators;
using MainMenu.Shop.Scripts;
using Services.LeaderBoards;
using Services.Localization.Scripts;
using Services.PlayerData;
using Services.PlayerData.Core.Language;
using UnityEngine;

namespace MainMenu.Infrasturcture
{
    public class Initializer: IBootstrapInitializer
    {
        private readonly MainMenuView _mainMenuView;
        private readonly ShopItemFactory _shopItemFactory;

        public Initializer(
            MainMenuView mainMenuView, 
            ShopItemFactory shopItemFactory)
        {
            _mainMenuView = mainMenuView;
            _shopItemFactory = shopItemFactory;
        }

        public IEnumerator Initialize(IDIContainer container)
        {
            IPlayerData playerData = container.Get<IPlayerData>();
            DeviceType deviceType = container.Get<IDeviceTypeHandler>().GetDeviceType();

            playerData.LoadProgressOfLevels();

            _mainMenuView.Init();
            
            yield return _mainMenuView.LevelSelector.Init(
                playerData.PassedLevels, 
                playerData.AvailableLevels, 
                container.Get<ILeaderBoardService>(), 
                container.Get<LevelsDatabase>().TutorialLevelId
            );

            ILanguageService languageService = container.Get<ILanguageService>();
            LanguageData[] languageDatas = container.Get<LanguageData[]>();
            
            _mainMenuView.LanguageSelectorMenu.Init(languageDatas, languageService.Language.Value);
            _mainMenuView.ShopView.Init(_shopItemFactory, container);
            _mainMenuView.TutorialView.Init(deviceType);
            //TODO заменить на сравнение с нужной платформой, я просто хз, какая стринга, в документации нет
            _mainMenuView.MainButtons.Init(true);

            SetupMediators(container);

            if (Application.isFocused)
                container.Get<PauseManager>().Resume();

            yield return new WaitForEndOfFrame();

            _mainMenuView.SettingsMenu.Init(container.Get<ICameraSettings>(), container.Get<ISoundSettings>());
            
            container.Get<ISoundController>().Play(SoundID.MainMenuMusic);
            ILoadingCurtainService loadingCurtainService = container.Get<ILoadingCurtainService>();
            loadingCurtainService.Hide();
        }
        
        private void SetupMediators(IDIContainer container)
        {
            var mainMenuMediator = new MainMenuMediator(container);
            var settingsMediator = new SettingsAndUIMediator(container);
            var coinsMediator = new CoinsMediator(container);
            var tutorialMediator = new TutorialMediator(container);
            var languageMediator = new LanguageMediator(container);
            var settingsSavingMediator = new GameSettingsSavingMediator(container);
            var settingsAndSoundMediator = new SettingsAndSoundMediator(container);
            var onSceneChangedDisposeContextMediator = new OnSceneChangedDisposeContextMediator(container, MainMenuTags.DISPOSABLES);
            
            var disposables = container.Get<CompositeDisposable>(MainMenuTags.DISPOSABLES);

            disposables.Add(mainMenuMediator);
            disposables.Add(settingsMediator);
            disposables.Add(coinsMediator);
            disposables.Add(tutorialMediator);
            disposables.Add(languageMediator);
            disposables.Add(settingsSavingMediator);
            disposables.Add(onSceneChangedDisposeContextMediator);
            disposables.Add(settingsAndSoundMediator);
        }
    }
}