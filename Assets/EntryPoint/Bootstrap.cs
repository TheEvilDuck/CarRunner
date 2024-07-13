using System.Collections;
using Common;
using Common.Data;
using Common.Data.Rewards;
using Common.Sound;
using DI;
using Levels;
using Services.PlayerInput;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EntryPoint
{
    public class Bootstrap
    {
        private const string BRAKE_BUTTON_RESOURCES_PATH = "Prefabs/Brake button";
        private const string LEVEL_DATABASE_PATH = "Levels database";
        private const string SOUND_CONTROLLER_PATH = "Prefabs/SoundController";
        private static Bootstrap _gameInstance;
        private DIContainer _projectContext;
        private Coroutines _coroutines;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void GameEntryPoint()
        {
            _gameInstance = new Bootstrap();
            _gameInstance.RunGame();
        }

        private void RunGame()
        {
            _projectContext = new DIContainer();

            _projectContext.Register(() => Resources.Load<LevelsDatabase>(LEVEL_DATABASE_PATH));
            _projectContext.Register(() => new GameSettings());
            _projectContext.Register(() => SetupPlayerData());
            _projectContext.Register(() => SetupInput());
            _projectContext.Register(() => SetupSoundController());
            _projectContext.Register(() => new RewardProvider());
            
            _coroutines = new GameObject("COROUTINES").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);

            Application.quitting += OnApplicationQuit;
            
            _coroutines.StartCoroutine(SceneSetup());

            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        private void OnSceneChanged(Scene previousScene, Scene nextScene)
        {
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
            var sceneBootstrap = Object.FindAnyObjectByType<MonoBehaviourBootstrap>();
            
            if (sceneBootstrap == null)
                return false;

            sceneBootstrap.Init(_projectContext);
            return true;
        }

        private IPlayerInput SetupInput()
        {
            IPlayerInput playerInput;

            #if !UNITY_EDITOR
                if (SystemInfo.deviceType== DeviceType.Desktop)
                    playerInput = new DesktopInput();
                else if (SystemInfo.deviceType == DeviceType.Handheld)
                    playerInput = new MobileInput(Resources.Load<RectTransform>(BRAKE_BUTTON_RESOURCES_PATH));
            #endif

            #if UNITY_EDITOR
                playerInput = new DesktopInput();
            #endif

            playerInput.Enable();
            return playerInput;
                    
        }

        private IPlayerData SetupPlayerData()
        {
            IPlayerData playerData = new PlayerDataPlayerPrefs();

            if (!playerData.LoadProgressOfLevels())
                playerData.AddAvailableLevel(_projectContext.Get<LevelsDatabase>().GetFirstLevel());

            return playerData;
        }

        private SoundController SetupSoundController()
        {
            SoundController prefab = Resources.Load<SoundController>(SOUND_CONTROLLER_PATH);
            SoundController soundController = Object.Instantiate(prefab);
            Object.DontDestroyOnLoad(soundController.gameObject);
            soundController.Init();

            return soundController;
        }
    }
}
