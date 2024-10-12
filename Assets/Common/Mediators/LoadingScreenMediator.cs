using System;
using DI;
using Services.SceneManagement;

namespace Common.Mediators
{
    public class LoadingScreenMediator : IDisposable
    {
        private readonly ISceneManager _sceneManager;
        private readonly LoadScreen _loadScreen;
        private readonly YandexGameFullScreenAd _yandexGameFullScreenAd;

        public LoadingScreenMediator(DIContainer context)
        {
            _sceneManager = context.Get<ISceneManager>();
            _loadScreen = context.Get<LoadScreen>();
            _yandexGameFullScreenAd = context.Get<YandexGameFullScreenAd>();

            _sceneManager.beforeSceneLoadingStarted += OnBeforeSceneLoading;
            _sceneManager.afterScemeLoadingEnd += OnAfterSceneLoaded;
            _yandexGameFullScreenAd.adShowingStarted += OnBeforeSceneLoading;
        }

        public void Dispose()
        {
            _sceneManager.beforeSceneLoadingStarted -= OnBeforeSceneLoading;
            _sceneManager.afterScemeLoadingEnd -= OnAfterSceneLoaded;
            _yandexGameFullScreenAd.adShowingStarted -= OnBeforeSceneLoading;
        }

        private void OnBeforeSceneLoading() => _loadScreen.Show();
        private void OnAfterSceneLoaded() => _loadScreen.Hide();
    }
}
