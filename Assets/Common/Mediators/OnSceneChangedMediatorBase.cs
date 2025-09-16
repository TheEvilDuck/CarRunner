using System;
using Infrastructure.DI;
using Services.SceneManagement;

namespace Common.Mediators
{
    public abstract class OnSceneChangedMediatorBase: IDisposable
    {
        private readonly ISceneManager _sceneManager;

        public OnSceneChangedMediatorBase(IDIContainer container)
        {
            _sceneManager = container.Get<ISceneManager>();

            _sceneManager.sceneLoadingRequested += OnSceneLoadingRequested;
        }

        public void Dispose() => _sceneManager.sceneLoadingRequested -= OnSceneLoadingRequested;
        protected abstract void OnSceneLoadingRequested(string sceneName);
    }
}