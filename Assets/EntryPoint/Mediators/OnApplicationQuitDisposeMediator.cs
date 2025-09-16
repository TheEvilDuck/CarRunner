using System;
using Common.Disposables;
using Infrastructure.DI;
using Services.ApplicationStatus;

namespace EntryPoint.Mediators
{
    public class OnApplicationQuitDisposeMediator: IDisposable
    {
        private readonly IApplicationStatusService _applicationStatusService;
        private readonly CompositeDisposable _compositeDisposable;

        public OnApplicationQuitDisposeMediator(
            IDIContainer container, 
            string compositeDisposableTag = null)
        {
            _applicationStatusService = container.Get<IApplicationStatusService>();
            _compositeDisposable = container.Get<CompositeDisposable>(compositeDisposableTag);

            _applicationStatusService.QuitStarted += OnQuitStarted;
        }

        public void Dispose() => _applicationStatusService.QuitStarted -= OnQuitStarted;
        private void OnQuitStarted() => _compositeDisposable?.Dispose();
    }
}