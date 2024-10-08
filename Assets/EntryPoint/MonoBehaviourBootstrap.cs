using System;
using System.Collections;
using DI;
using Services.SceneManagement;
using UnityEngine;

namespace EntryPoint
{
    public abstract class MonoBehaviourBootstrap : MonoBehaviour
    {
        protected DIContainer _sceneContext;
        protected bool _inited;
        protected event Action _delayedStart;
        private DIContainer _projectContext;

        public void Init(DIContainer projectContext)
        {
            _projectContext = projectContext;
            _sceneContext =  new DIContainer(_projectContext);
            _projectContext.Get<ISceneManager>().beforeSceneLoadingStarted += OnBeforeSceneChanged;
            Setup();
            _inited = true;
        }

        private void OnDestroy() 
        {
            _projectContext.Get<ISceneManager>().beforeSceneLoadingStarted -= OnBeforeSceneChanged;
        }

        private IEnumerator Start() 
        {
            while (!_inited)
            {
                yield return new WaitForSeconds(0.1f);
            }

            _delayedStart?.Invoke();
        }

        protected virtual void Setup() {}
        protected virtual void OnBeforeSceneChanged() 
        {
            _projectContext.Get<ISceneManager>().beforeSceneLoadingStarted -= OnBeforeSceneChanged;
        }
    }

}