using Common.CoroutinePerformer;
using Infrastructure.Bootstraps;
using Infrastructure.DI;
using UnityEngine;

namespace EntryPoint
{
    public static class GameEntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Run()
        {
            Application.runInBackground = true;
            Application.targetFrameRate = 60;

            DIContainer projectContext = new DIContainer();
            ICoroutinePerformer coroutinePerformer = SetupCoroutines();
            projectContext.Register(coroutinePerformer);
            
            GameEntryPointRegistrator gameEntryPointRegistrator = new GameEntryPointRegistrator();
            GameEntryPointInitializer gameEntryPointInitializer = new GameEntryPointInitializer();
            Bootstrap bootstrap = new Bootstrap(gameEntryPointRegistrator, gameEntryPointInitializer, projectContext);

            coroutinePerformer.StartCoroutine(bootstrap.Run());
        }
        
        private static ICoroutinePerformer SetupCoroutines()
        {
            CoroutinePerformer coroutinePerformer = new GameObject("CoroutinePerformer").AddComponent<CoroutinePerformer>();
            Object.DontDestroyOnLoad(coroutinePerformer.gameObject);

            return coroutinePerformer;
        }
    }
}