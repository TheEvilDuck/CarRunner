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
using Configs;
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
using Services.PlayerData.Core;
using Services.PlayerData.Core.Language;
using Services.PlayerData.Core.Levels;
using Services.PlayerData.Core.Wallet;
using Services.PlayerData.Implementation.PlayerPrefsData;
using Services.PlayerData.Implementation.YandexCloud;
using Services.PlayerData.SavingStrategy;
using Services.PurchaseService;
using Services.PurchaseService.FakeStorePurchases;
using Services.PurchaseService.YandexPurchases;
using Services.RewardProvider;
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
        private const string WALLET_CONFIG_PATH = "WalletConfig";
        
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
            container.Register(() => SetupRewardProvider(container));
            container.Register(() => SetupPause(container));
            //container.Register(SetupImageLoadYG);
            container.Register(SetupSceneManager);
            
            container.Register(() => SetupYandexFuulScreenAd(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);
            
            container.Register(() => SetupPlayerData(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);

            container.Register(SetupInput)
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG)
                .AddToTickables(EntryPointTags.PROJECT_TICKABLES_TAG);
            
            container.Register(SetupLocalizationService);
            container.Register(SetupPreferedLanguageService);
            
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
                .AddToTickables(EntryPointTags.PROJECT_TICKABLES_TAG);

            container.Register(() => SetupLeaderBoardService(container));
            
            container.Register(() => SetupYandexGameIntegrator(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG)
                .NonLazy();
            
            container.Register(SetupPurchaseService)
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);

            container.Register(SetupTimerService);
            container.Register(() => SetupAvailableLevelsService(container));

            container.Register(SetupWalletConfig);

            container.Register(SetupWalletDataProvider);
            container.Register(SetupLevelsDataProvider);
            container.Register(SetupLanguageDataProvider);

            container.Register(() => SetupWalletService(container))
                .InAdditionRegisterAs<IWalletService>()
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);

            container.Register(() => SetupLevelsService(container))
                .InAdditionRegisterAs<ILevelsService>();

            container.Register(() => SetupLanguageService(container))
                .InAdditionRegisterAs<ILanguageService>();

            container.Register(() => SetupSaveLoadService(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);

            container.Register(() => SetupDataChangedSavingStrategy(container))
                .AddToDisposables(EntryPointTags.PROJECT_DISPOSABLES_TAG);
        }

        private IApplicationStatusService SetupApplicationStatusService() => new ApplicationStatusService();
        private ILoadingCurtainService SetupLoadingCurtain() => new LoadingCurtainService();

        private IRewardProvider SetupRewardProvider(IDIContainer container)
        {
            IPlayerData playerData = container.Get<IPlayerData>();
            LevelsDatabase levelsDatabase = container.Get<LevelsDatabase>();
            return new RewardProvider(playerData, levelsDatabase);
        }

        private TickableManagerFactory SetupTickableManagerFactory(IDIContainer container)
        {
            ICoroutinePerformer coroutinePerformer = container.Get<ICoroutinePerformer>();
            return new TickableManagerFactory(coroutinePerformer);
        }

        private TickableManager SetupProjectTickables(IDIContainer container)
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
        
        private ILocalizationService SetupLocalizationService()
            => Resources.Load<SOLocalizationService>("SO localization service");

        private IPreferedLanguageService SetupPreferedLanguageService()
            => new ConstantPreferedLanguageService();
        
        private Localizator SetupLocalizator(IDIContainer container)
        {
            ILocalizationService localizationService = container.Get<ILocalizationService>();
            ILanguageService languageService = container.Get<ILanguageService>();
            return new Localizator(localizationService, languageService);
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

        private IWalletConfig SetupWalletConfig() => Resources.Load<WalletConfig>(WALLET_CONFIG_PATH);

        private AvailableLevelsService SetupAvailableLevelsService(IDIContainer container)
        {
            LevelsDatabase levelsDatabase = container.Get<LevelsDatabase>();
            ILevelsService levelsService = container.Get<ILevelsService>();
            return new AvailableLevelsService(levelsDatabase, levelsService);
        }

        private IDataProvider<IWalletData> SetupWalletDataProvider() => new PlayerPrefsWalletDataProvider();
        private IDataProvider<ILevelsData> SetupLevelsDataProvider() => new PlayerPrefsLevelsDataProvider();
        private IDataProvider<ILanguageData> SetupLanguageDataProvider() => new PlayerPrefsLanguageDataProvider();

        private WalletService SetupWalletService(IDIContainer container)
        {
            IWalletConfig config = container.Get<IWalletConfig>();
            IDataProvider<IWalletData> dataProvider = container.Get<IDataProvider<IWalletData>>();
            ICoroutinePerformer coroutinePerformer = container.Get<ICoroutinePerformer>();
            return new WalletService(dataProvider, coroutinePerformer, config);
        }

        private LevelsService SetupLevelsService(IDIContainer container)
        {
            IDataProvider<ILevelsData> dataProvider = container.Get<IDataProvider<ILevelsData>>();
            ICoroutinePerformer coroutinePerformer = container.Get<ICoroutinePerformer>();
            LevelsDatabase levelsDatabase = container.Get<LevelsDatabase>();
            return new LevelsService(dataProvider, coroutinePerformer, levelsDatabase);
        }

        private LanguageService SetupLanguageService(IDIContainer container)
        {
            IDataProvider<ILanguageData> dataProvider = container.Get<IDataProvider<ILanguageData>>();
            ICoroutinePerformer coroutinePerformer = container.Get<ICoroutinePerformer>();
            IPreferedLanguageService preferedLanguageService = container.Get<IPreferedLanguageService>();
            return new LanguageService(dataProvider, coroutinePerformer, preferedLanguageService);
        }

        private SaveLoadService SetupSaveLoadService(IDIContainer container)
        {
            ICoroutinePerformer coroutinePerformer = container.Get<ICoroutinePerformer>();
            return new SaveLoadService(coroutinePerformer);
        }

        private DataChangedSavingStrategy SetupDataChangedSavingStrategy(IDIContainer container)
        {
            WalletService walletService = container.Get<WalletService>();
            return new DataChangedSavingStrategy(walletService);
        }
    }
}