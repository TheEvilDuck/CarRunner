using System;
using System.Collections;
using Common.CoroutinePerformer;
using Infrastructure.DI;
using Services.SceneManagement;
using UnityEngine;

namespace Infrastructure.Bootstraps
{
    public class MonoBootstrapStarter: IDisposable
    {
        private readonly ISceneManager _sceneManager;
        private readonly IDIContainer _diContainer;
        private readonly ICoroutinePerformer _coroutinePerformer;

        public MonoBootstrapStarter(IDIContainer diContainer)
        {
            _diContainer = diContainer;
            
            _sceneManager = _diContainer.Get<ISceneManager>();
            _coroutinePerformer = _diContainer.Get<ICoroutinePerformer>();

            _sceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Dispose()
        {
            _sceneManager.sceneLoaded -= OnSceneLoaded;
        }
        
        private void OnSceneLoaded(string sceneName)
        {
            MonoBehaviourBootstrap monoBehaviourBootstrap = GameObject.FindAnyObjectByType<MonoBehaviourBootstrap>();
            Debug.Log("AAA");

            if (monoBehaviourBootstrap == null)
                return;

            _coroutinePerformer.StartCoroutine(SetupAsync(monoBehaviourBootstrap));
        }

        private IEnumerator SetupAsync(MonoBehaviourBootstrap monoBehaviourBootstrap)
        {
            Bootstrap bootstrap = new Bootstrap(monoBehaviourBootstrap, monoBehaviourBootstrap, _diContainer);
            yield return bootstrap.Run();
        }
    }
}