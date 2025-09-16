using System;
using Common.Sounds.Scripts;
using Infrastructure.DI;
using Services.SceneManagement;

namespace EntryPoint.Mediators
{
    public class SoundAndSceneChangingMediator: IDisposable
    {
        private readonly ISceneManager _sceneManager;
        private readonly ISoundController _soundController;

        public SoundAndSceneChangingMediator(IDIContainer container)
        {
            _sceneManager = container.Get<ISceneManager>();
            _soundController = container.Get<ISoundController>();

            _sceneManager.sceneLoadingRequested += OnSceneLoadingRequested;
        }

        public void Dispose() => _sceneManager.sceneLoadingRequested -= OnSceneLoadingRequested;
        
        private void OnSceneLoadingRequested(string sceneName) => _soundController.StopAll();
    }
}