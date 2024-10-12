using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Data;
using Common.Data.Rewards;
using Common.Disposables;
using Common.Mediators;
using Common.Sound;
using DI;
using Gameplay.UI;
using Levels;
using Services.Localization;
using Services.PlayerInput;
using Services.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace EntryPoint
{
    public class Bootstrap
    {
        public const string PLATFORM_DI_TAG = "platform";
        public const string PROJECT_DISPOSABLES_TAG = nameof(PROJECT_DISPOSABLES_TAG);
        private const string BRAKE_BUTTON_RESOURCES_PATH = "Prefabs/BrakeButton";
        private const string LEVEL_DATABASE_PATH = "Levels database";
        private const string SOUND_CONTROLLER_PATH = "Prefabs/SoundController";
        private static Bootstrap _gameInstance;
        private DIContainer _projectContext;
        private List<GameObject> _dontDestroyOnLoadObjects;
        private Coroutine _tickablesCoroutine;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void GameEntryPoint()
        {
            _gameInstance = new Bootstrap();
            _gameInstance.RunGame();
        }

        private void RunGame()
        {
            Application.runInBackground = true;
            Application.targetFrameRate = 60;

            _projectContext = new DIContainer();
            _dontDestroyOnLoadObjects = new List<GameObject>();
            
            _projectContext.Register(SetupLoadScreen);
            _projectContext.Register(() => new CompositeDisposable(), PROJECT_DISPOSABLES_TAG);
            _projectContext.Register(SetupSoundController);
            _projectContext.Register(SetupCoroutines);
            _projectContext.Register(() => Resources.Load<LevelsDatabase>(LEVEL_DATABASE_PATH));
            _projectContext.Register(() => new GameSettings());
            _projectContext.Register<ISettings>(() => _projectContext.Get<GameSettings>());
            _projectContext.Register<ISoundSettings>(() => _projectContext.Get<GameSettings>());
            _projectContext.Register<ICameraSettings>(() => _projectContext.Get<GameSettings>());
            _projectContext.Register(() => new RewardProvider());
            _projectContext.Register(SetupImageLoadYG);
            _projectContext.Register(SetupSceneManager);
            // SetupDeviceType() инициализируется как инстанс, поскольку DeviceType это энам и не может быть null
            // А DI контейнер использует делегат только тогда, когда инстанс объекта null
            // надо будет предусмотреть инициализацию структур и энамов
            _projectContext.Register(SetupDeviceType());
            _projectContext.Register(SetupPause);
            _projectContext.Register(SetupTickables).NonLazy();
            
            if (_projectContext.Get<DeviceType>() == DeviceType.Handheld)
                _projectContext.Register(SetupBrakeButton);

            Application.quitting += OnApplicationQuit;
            Application.focusChanged += OnFocusChanged;

            SceneManager.activeSceneChanged += OnSceneChanged;

            YandexGame.GetDataEvent += PluginYGInit;
            YandexGame.GetPaymentsEvent += OnPaymentsGot;
            YandexGame.OpenFullAdEvent += OnFullVideoOpenEvent;
        }

        private void PluginYGInit()
        {
            _projectContext.Register(SetupYandexFuulScreenAd);
            _projectContext.Register(SetupPlayerData);
            _projectContext.Register(SetupInput);
            _projectContext.Register(SetupLocalizationService);
            _projectContext.Register(SetupLocalizator);
            _projectContext.Register(SetupLocalizationRegistrator).NonLazy();
            _projectContext.Register(() => Resources.LoadAll<LanguageData>(""));
            _projectContext.Register(SetupPlatform, PLATFORM_DI_TAG);
            _projectContext.Register(SetupLeaderboardData);

            SetupMediators();

            SceneSetup();
            
            YandexGame.GameReadyAPI();
            YandexGame.GetDataEvent -= PluginYGInit;

            OnFocusChanged(true);
        }

        private void OnFullVideoOpenEvent() => _projectContext.Get<PauseManager>().Pause();

        private void OnSceneChanged(Scene previousScene, Scene nextScene)
        {
            _projectContext.Get<SoundController>().StopAll();
            
            InitSceneBootstrap();
        }

        private void OnFocusChanged(bool isFocused)
        {
            if (!isFocused)
                _projectContext.Get<PauseManager>().Pause();
            else
                _projectContext.Get<PauseManager>().Resume();
        }

        private void OnPaymentsGot()
        {
            _projectContext.Register(() => YandexGame.purchases);
        }

        private List<ITickable> SetupTickables()
        {
            var tickables = new List<ITickable>();

            Action coroutine = () =>
            {
                tickables.ForEach((x) => x?.Tick(Time.deltaTime));
            };

            _tickablesCoroutine = _projectContext.Get<Coroutines>().StartCoroutine(TickTickables(tickables));

            return tickables;
        }

        private YandexGameFullScreenAd SetupYandexFuulScreenAd()
        {
            YandexGameFullScreenAd yandexGameFullScreenAd = new YandexGameFullScreenAd();
            _projectContext.Get<CompositeDisposable>(PROJECT_DISPOSABLES_TAG).Add(yandexGameFullScreenAd);

            return yandexGameFullScreenAd;
        }

        private ImageLoadYG SetupImageLoadYG()
        {
            var prefab = Resources.Load<ImageLoadYG>("YGImageLoader");
            var obj = UnityEngine.GameObject.Instantiate(prefab);
            UnityEngine.GameObject.DontDestroyOnLoad(obj);
            return obj;
        }

        private ILeaderBoardData SetupLeaderboardData()
        {
            YandexCloudLeaderboard yandexCloudLeaderboard = new YandexCloudLeaderboard();

            _projectContext.Get<List<ITickable>>().Add(yandexCloudLeaderboard);
            _projectContext.Get<CompositeDisposable>(PROJECT_DISPOSABLES_TAG);

            return yandexCloudLeaderboard;
        }

        private IEnumerator TickTickables(List<ITickable> tickables)
        {
            while (true)
            {
                tickables.ForEach((x) => x?.Tick(Time.deltaTime));
                yield return null;
            }
        }

        private void OnApplicationQuit()
        {
            _projectContext.Get<ISettings>().SaveSettings();
            _projectContext.Get<CompositeDisposable>(PROJECT_DISPOSABLES_TAG)?.Dispose();

            foreach (GameObject gameObject in _dontDestroyOnLoadObjects)
            {
                GameObject.Destroy(gameObject);
            }

            _projectContext.Get<Coroutines>().StopCoroutine(_tickablesCoroutine);
            SceneManager.activeSceneChanged -= OnSceneChanged;
            Application.quitting -= OnApplicationQuit;
            Application.focusChanged -= OnFocusChanged;
            YandexGame.OpenFullAdEvent -= OnFullVideoOpenEvent;
        }

        private async void SceneSetup()
        {
            if (!InitSceneBootstrap())
                await _projectContext.Get<ISceneManager>().LoadScene(SceneIDs.MAIN_MENU);
        }

        private bool InitSceneBootstrap()
        {
            var sceneBootstrap = UnityEngine.Object.FindAnyObjectByType<MonoBehaviourBootstrap>();

            if (sceneBootstrap == null)
                return false;

            sceneBootstrap.Init(_projectContext);
            return true;
        }

        private LoadScreen SetupLoadScreen()
        {
            LoadScreen loadScreen = new LoadScreen();
            _dontDestroyOnLoadObjects.Add(loadScreen.Screen);

            return loadScreen;
        }

        private ISceneManager SetupSceneManager()
        {
            return new SimpleUnitySceneManager();
        }

        private Coroutines SetupCoroutines()
        {
            var coroutines = new GameObject("COROUTINES").AddComponent<Coroutines>();
            UnityEngine.Object.DontDestroyOnLoad(coroutines.gameObject);
            _dontDestroyOnLoadObjects.Add(coroutines.gameObject);

            return coroutines;
        }

        private IPlayerInput SetupInput()
        {
            IPlayerInput playerInput;
            DeviceType deviceType = _projectContext.Get<DeviceType>();

            if (deviceType == DeviceType.Desktop)
                playerInput = new DesktopInput();
            else if (deviceType == DeviceType.Handheld)
                playerInput = new MobileInput(() => _projectContext.Get<IBrakeButton>());
            else
                throw new ArgumentException($"Unknown device type");

            playerInput.Enable();

            return playerInput;
        }

        private IPlayerData SetupPlayerData()
        {
            IPlayerData playerData;

            if (YandexGame.SDKEnabled)
            {
                playerData = new YandexCloudPlayerData();
                _projectContext.Get<CompositeDisposable>(PROJECT_DISPOSABLES_TAG).Add(playerData as IDisposable);
            }
            else
            {
                playerData = new PlayerDataPlayerPrefs();
            }

            playerData.AddAvailableLevel(_projectContext.Get<LevelsDatabase>().GetFirstLevel());
            playerData.AddAvailableLevel(_projectContext.Get<LevelsDatabase>().TutorialLevelId);

            return playerData;
        }

        private IBrakeButton SetupBrakeButton()
        {
            return GameObject.Instantiate(Resources.Load<BrakeButton>(BRAKE_BUTTON_RESOURCES_PATH));
        }

        private string SetupPlatform()
        {
            return YandexGame.EnvironmentData.platform;
        }

        private DeviceType SetupDeviceType()
        {
            DeviceType deviceType = DeviceType.Desktop;

#if UNITY_WEBGL
            if (YandexGame.EnvironmentData.isDesktop)
                deviceType = DeviceType.Desktop;
            else if (YandexGame.EnvironmentData.isMobile)
                deviceType = DeviceType.Handheld;
#endif

#if UNITY_STANDALONE_WIN
            deviceType = DeviceType.Desktop;
#endif

#if UNITY_ANDROID
            deviceType = DeviceType.Handheld;
#endif

#if UNITY_EDITOR
            deviceType = DeviceType.Desktop;
            //deviceType = DeviceType.Handheld;
#endif

            return deviceType;
        }

        private SoundController SetupSoundController()
        {
            SoundController prefab = Resources.Load<SoundController>(SOUND_CONTROLLER_PATH);
            SoundController soundController = UnityEngine.Object.Instantiate(prefab);
            UnityEngine.Object.DontDestroyOnLoad(soundController.gameObject);
            soundController.Init();
            _dontDestroyOnLoadObjects.Add(soundController.gameObject);
            _projectContext.Get<CompositeDisposable>(PROJECT_DISPOSABLES_TAG).Add(soundController);

            return soundController;
        }

        private PauseManager SetupPause()
        {
            var pauseManager = new PauseManager();
            pauseManager.Register(_projectContext.Get<SoundController>());
            pauseManager.Register(_projectContext.Get<IPlayerInput>());
            return pauseManager;
        }

        private ILocalizationService SetupLocalizationService()
        {
            var service = Resources.Load<SOLocalizationService>("SO localization service");
            
            string currentLanguage = _projectContext.Get<IPlayerData>().SavedPreferdLanguage.Value;

            if (string.IsNullOrEmpty(currentLanguage))
            {
                currentLanguage = service.CurrentLanguage;
                _projectContext.Get<IPlayerData>().SaveLanguage(currentLanguage);
            }

            service.SetLanguage(currentLanguage);

            _projectContext.Get<IPlayerData>().SavedPreferdLanguage.changed += service.SetLanguage;

            return service;
        }

        private Localizator SetupLocalizator()
        {
            Localizator localizator = new Localizator(_projectContext.Get<ILocalizationService>());
            return localizator;
        }

        private LocalizationRegistrator SetupLocalizationRegistrator()
        {
            LocalizationRegistrator localizationRegistrator = new LocalizationRegistrator(_projectContext.Get<Localizator>());
            return localizationRegistrator;
        }

        private void SetupMediators()
        {
            var coinsLeaderboardMediator = new CoinsLeaderboardMediator(_projectContext);
            var loadingScreenMediator = new LoadingScreenMediator(_projectContext);

            _projectContext.Get<CompositeDisposable>(PROJECT_DISPOSABLES_TAG).Add(coinsLeaderboardMediator);
            _projectContext.Get<CompositeDisposable>(PROJECT_DISPOSABLES_TAG).Add(loadingScreenMediator);
        }
    }
}