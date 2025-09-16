using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.CoroutinePerformer;
using Common.DeviceTypeHandling;
using Common.Disposables;
using Common.LoadingCurtain;
using Common.Settings;
using Common.Sounds.Scripts;
using Common.Tickables;
using Infrastructure.Bootstraps;
using Infrastructure.DI;
using Levels;
using Levels.Scripts;
using Services.Ads;
using Services.ApplicationStatus;
using Services.InputService;
using Services.Integrations;
using Services.LeaderBoards;
using Services.Localization;
using Services.Localization.Scripts;
using Services.PlayerData;
using Services.PlayerData.Rewards;
using Services.PurchaseService;
using Services.PurchaseService.FakeStorePurchases;
using Services.PurchaseService.YandexPurchases;
using Services.SceneManagement;
using Services.TimerService;
using UnityEngine;
using YG;

namespace EntryPoint
{
    public class GameEntryPointRegistrator: IDIRegistrator
    {
        private const string SOUND_CONTROLLER_PATH = "Prefabs/SoundController";
        private const string LEVEL_DATABASE_PATH = "Levels database";
        
        public void MakeRegistrationsInto(DIContainer container)
        {
            container.Register(() => new CompositeDisposable(), EntryPointTags.PROJECT_DISPOSABLES_TAG);
            container.Register(SetupLoadingCurtain);
            
            container.Register(SetupSoundController)
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);
            
            container.Register(() => Resources.Load<LevelsDatabase>(LEVEL_DATABASE_PATH));
            container.Register(() => new GameSettings());
            container.Register<ISettings>(() => container.Get<GameSettings>());
            container.Register<ISoundSettings>(() => container.Get<GameSettings>());
            container.Register<ICameraSettings>(() => container.Get<GameSettings>());
            container.Register(() => new RewardProvider());
            container.Register(() => SetupPause(container));
            //container.Register(SetupImageLoadYG);
            container.Register(SetupSceneManager);
            
            container.Register(() => SetupYandexFuulScreenAd(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);
            
            container.Register(() => SetupPlayerData(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);

            container.Register(SetupInput)
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG)
                .AddToTickablesNonLazy(EntryPointTags.PROJECT_TICKABLES_TAG);
            
            container.Register(() => SetupLocalizationService(container));
            
            container.Register(SetupDeviceTypeHandler);
            
            
            container.Register(() => SetupLocalizator(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);
            
            container.Register(() => SetupLocalizationRegistrator(container))
                .NonLazy();
            
            container.Register(() => Resources.LoadAll<LanguageData>(""));
            
            container.Register(() => new MonoBootstrapStarter(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG)
                .NonLazy();
            
            container.Register(SetupApplicationStatusService)
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);
            
            container.Register(() => SetupProjectTickables(container), EntryPointTags.PROJECT_TICKABLES_TAG);
            container.Register(() => SetupTickableManagerFactory(container));
            
            container.Register(() => SetupLeaderBoardProvider(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG)
                .AddToTickablesNonLazy(EntryPointTags.PROJECT_TICKABLES_TAG);

            container.Register(() => SetupLeaderBoardService(container));
            container.Register(() => SetupYandexGameIntegrator(container));
            
            container.Register(SetupPurchaseService)
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);

            container.Register(SetupTimerService);
        }

        private IApplicationStatusService SetupApplicationStatusService() => new ApplicationStatusService();
        private ILoadingCurtainService SetupLoadingCurtain() => new LoadingCurtainService();

        private TickableManagerFactory SetupTickableManagerFactory(IDIContainer container)
        {
            ICoroutinePerformer coroutinePerformer = container.Get<ICoroutinePerformer>();
            return new TickableManagerFactory(coroutinePerformer);
        }

        private ITickableManager SetupProjectTickables(IDIContainer container)
        {
            TickableManagerFactory tickableManagerFactory = container.Get<TickableManagerFactory>();
            return tickableManagerFactory.CreateWithCoroutinePerformer();
        }
        
        private ISceneManager SetupSceneManager() => new SimpleUnitySceneManager();

        private IDeviceTypeHandler SetupDeviceTypeHandler() => new DeviceTypeHandler();
        
        private PlayerInput SetupInput()
        {
            return new PlayerInput();
        }
        
        private ISoundController SetupSoundController()
        {
            SoundController prefab = Resources.Load<SoundController>(SOUND_CONTROLLER_PATH);
            SoundController soundController = UnityEngine.Object.Instantiate(prefab);
            UnityEngine.Object.DontDestroyOnLoad(soundController.gameObject);

            return soundController;
        }
        
        private PauseManager SetupPause(IDIContainer container)
        {
            var pauseManager = new PauseManager();
            pauseManager.Register(container.Get<ISoundController>());
            pauseManager.Register(container.Get<PlayerInput>());
            return pauseManager;
        }
        
        private ILocalizationService SetupLocalizationService(IDIContainer container)
        {
            var service = Resources.Load<SOLocalizationService>("SO localization service");
            
            string currentLanguage = container.Get<IPlayerData>().SavedPreferdLanguage.Value;

            if (string.IsNullOrEmpty(currentLanguage))
            {
                currentLanguage = service.CurrentLanguage;
                container.Get<IPlayerData>().SaveLanguage(currentLanguage);
            }

            service.SetLanguage(currentLanguage);

            container.Get<IPlayerData>().SavedPreferdLanguage.changed += service.SetLanguage;

            return service;
        }
        
        private Localizator SetupLocalizator(IDIContainer container)
        {
            Localizator localizator = new Localizator(container.Get<ILocalizationService>());
            return localizator;
        }
        
        private LocalizationRegistrator SetupLocalizationRegistrator(IDIContainer container)
        {
            Localizator localizator = container.Get<Localizator>();
            LocalizationRegistrator localizationRegistrator = new LocalizationRegistrator(localizator);
            return localizationRegistrator;
        }
        
        private IAdsService SetupYandexFuulScreenAd(IDIContainer container)
        {
            ITimerService timerService = container.Get<ITimerService>();
            YandexGameFullScreenAd yandexGameFullScreenAd = new YandexGameFullScreenAd(timerService);

            return yandexGameFullScreenAd;
        }
        
        private ILeaderBoardProvider SetupLeaderBoardProvider(IDIContainer container)
        {
            ICoroutinePerformer coroutinePerformer = container.Get<ICoroutinePerformer>();
            return new YandexCloudLeaderboardProvider(coroutinePerformer);
        }

        private ILeaderBoardService SetupLeaderBoardService(IDIContainer container)
        {
            ILeaderBoardProvider leaderBoardProvider = container.Get<ILeaderBoardProvider>();
            return new LeaderBoardService(leaderBoardProvider);
        }
        
        private IPlayerData SetupPlayerData(IDIContainer container)
        {
            IPlayerData playerData;

            if (YandexGame.SDKEnabled)
            {
                playerData = new YandexCloudPlayerData();
            }
            else
            {
                playerData = new PlayerDataPlayerPrefs();
            }

            playerData.AddAvailableLevel(container.Get<LevelsDatabase>().GetFirstLevel());
            playerData.AddAvailableLevel(container.Get<LevelsDatabase>().TutorialLevelId);

            return playerData;
        }

        private YandexGameIntegrator SetupYandexGameIntegrator(IDIContainer container)
        {
            PauseManager pauseManager = container.Get<PauseManager>();
            return new YandexGameIntegrator(pauseManager);
        }

        private IPurchaseService SetupPurchaseService() => new FakeStorePurchaseService();
        private ITimerService SetupTimerService() => new TimerService();
    }
}