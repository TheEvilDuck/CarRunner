using System;
using Common.Disposables;
using Infrastructure.DI;
using Services.SceneManagement;

namespace Common.Mediators
{
    public class OnSceneChangedDisposeContextMediator: OnSceneChangedMediatorBase
    {
        private readonly CompositeDisposable _disposables;

        public OnSceneChangedDisposeContextMediator(IDIContainer container, string disposableTag) : base(container)
        {
            _disposables = container.Get<CompositeDisposable>(disposableTag);
        }

        protected override void OnSceneLoadingRequested(string sceneName) => _disposables?.Dispose();
    }
}