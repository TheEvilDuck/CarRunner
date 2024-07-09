using System.Collections;
using Common;
using DI;
using Services.PlayerInput;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EntryPoint
{
    public class Bootstrap
    {
        private const string BRAKE_BUTTON_RESOURCES_PATH = "Prefabs/Brake button.prefab";
        private static Bootstrap _gameInstance;
        private DIContainer _projectContext;
        private Coroutines _coroutines;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void GameEntryPoint()
        {
            _gameInstance = new Bootstrap();
            _gameInstance.RunGame();
        }

        private void RunGame()
        {
            _projectContext = new DIContainer();

            _projectContext.Register(() => new GameSettings());
            _projectContext.Register(() => new PlayerData());
            _projectContext.Register(() => SetupInput());

            _coroutines = new GameObject("COROUTINES").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines.gameObject);

            SceneManager.activeSceneChanged += OnSceneChanged;
            Application.quitting += OnApplicationQuit;
            
            _coroutines.StartCoroutine(SceneSetup());
        }

        private void OnSceneChanged(Scene previousScene, Scene nextScene)
        {
            _coroutines.StartCoroutine(SceneSetup());
        }

        private void OnApplicationQuit()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            Application.quitting -= OnApplicationQuit;
        }

        private IEnumerator SceneSetup()
        {
            var sceneBootstrap = Object.FindAnyObjectByType<MonoBehaviourBootstrap>();

            if (sceneBootstrap == null)
            {
                yield return SceneManager.LoadSceneAsync(SceneIDs.MAIN_MENU);
                sceneBootstrap = Object.FindAnyObjectByType<MonoBehaviourBootstrap>();
            }

            sceneBootstrap.Init(_projectContext);
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
    }
}
