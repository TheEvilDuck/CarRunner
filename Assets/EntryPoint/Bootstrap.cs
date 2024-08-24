using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using Common.Data;
using Common.Data.Rewards;
using Common.Sound;
using DI;
using Gameplay.UI;
using Levels;
using Services.Localization;
using Services.PlayerInput;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

namespace EntryPoint
{
    public class Bootstrap
    {
        private const string BRAKE_BUTTON_RESOURCES_PATH = "Prefabs/BrakeButton";
        private const string LEVEL_DATABASE_PATH = "Levels database";
        private const string SOUND_CONTROLLER_PATH = "Prefabs/SoundController";
        private const string LOADING_SCREEN = "Loading screen";
        private static Bootstrap _gameInstance;
        private DIContainer _projectContext;
        private Coroutines _coroutines;
        private List<IDisposable> _disposables;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void GameEntryPoint()
        {
            _gameInstance = new Bootstrap();
            _gameInstance.RunGame();
        }

        ~Bootstrap()
        {
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();
        }

        private void RunGame()
        {
            LoadScreen();
            
            _projectContext = new DIContainer();
            _disposables = new List<IDisposable>();

            _projectContext.Register(() => Resources.Load<LevelsDatabase>(LEVEL_DATABASE_PATH));
            _projectContext.Register(() => new GameSettings());
            _projectContext.Register<ISoundSettings>(() => _projectContext.Get<GameSettings>());
            _projectContext.Register<ICameraSettings>(() => _projectContext.Get<GameSettings>());
            _projectContext.Register(() => SetupSoundController());
            _projectContext.Register(() => new RewardProvider());
            _projectContext.Register(SetupDeviceType());
            _projectContext.Register(SetupBrakeButton);

            _coroutines = new GameObject("COROUTINES").AddComponent<Coroutines>();
            UnityEngine.Object.DontDestroyOnLoad(_coroutines.gameObject);

            Application.quitting += OnApplicationQuit;

            SceneManager.activeSceneChanged += OnSceneChanged;

            YandexGame.GetDataEvent += PluginYGInit;
        }

        private void PluginYGInit()
        {
            _projectContext.Register(SetupPlayerData);
            _projectContext.Register(SetupInput);
            _projectContext.Register(new YandexGameLocalization());
            _projectContext.Register(SetupLocalizationService);
            _projectContext.Register(SetupLocalizator);
            _projectContext.Register(SetupLocalizationRegistrator).NonLazy();
            _projectContext.Register(() => Resources.LoadAll<LanguageData>(""));
            _coroutines.StartCoroutine(SceneSetup());
            YandexGame.GameReadyAPI();
            _projectContext.Get<YandexGameLocalization>().SetupYGLocalization();
            YandexGame.GetDataEvent -= PluginYGInit;
        }

        private void OnSceneChanged(Scene previousScene, Scene nextScene)
        {
            _projectContext.Get<SoundController>().StopAll();
            
            InitSceneBootstrap();
        }

        private void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            Application.quitting -= OnApplicationQuit;
        }

        private IEnumerator SceneSetup()
        {
            if (!InitSceneBootstrap())
                yield return SceneManager.LoadSceneAsync(SceneIDs.MAIN_MENU);
        }

        private bool InitSceneBootstrap()
        {
            var sceneBootstrap = UnityEngine.Object.FindAnyObjectByType<MonoBehaviourBootstrap>();

            if (sceneBootstrap == null)
                return false;

            sceneBootstrap.Init(_projectContext);
            return true;
        }

        private void LoadScreen()
        {
            if (SceneManager.GetActiveScene().name == SceneIDs.BOOTSTRAP)
            {
                new GameObject("Camera").AddComponent<Camera>();
                MonoBehaviour.Instantiate(Resources.Load<Canvas>(LOADING_SCREEN));
            }
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
                _disposables.Add(playerData as IDisposable);
            }
            else
            {
                playerData = new PlayerDataPlayerPrefs();
                playerData.LoadProgressOfLevels();
            }

            playerData.AddAvailableLevel(_projectContext.Get<LevelsDatabase>().GetFirstLevel());

            return playerData;
        }

        private IBrakeButton SetupBrakeButton()
        {
            return GameObject.Instantiate(Resources.Load<BrakeButton>(BRAKE_BUTTON_RESOURCES_PATH));
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
            deviceType = DeviceType.Handheld;
#endif

            return deviceType;
        }

        private SoundController SetupSoundController()
        {
            SoundController prefab = Resources.Load<SoundController>(SOUND_CONTROLLER_PATH);
            SoundController soundController = UnityEngine.Object.Instantiate(prefab);
            UnityEngine.Object.DontDestroyOnLoad(soundController.gameObject);
            soundController.Init();

            return soundController;
        }

        private ILocalizationService SetupLocalizationService()
        {
            var service = Resources.Load<SOLocalizationService>("SO localization service");
            
            string currentLanguage = _projectContext.Get<IPlayerData>().SavedPreferedLanguage;

            if (string.IsNullOrEmpty(currentLanguage))
            {
                currentLanguage = service.CurrentLanguage;
            }

            service.SetLanguage(currentLanguage);

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
    }
}