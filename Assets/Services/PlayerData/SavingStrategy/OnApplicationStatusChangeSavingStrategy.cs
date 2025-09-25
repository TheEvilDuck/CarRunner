using System;
using Services.ApplicationStatus;

namespace Services.PlayerData.SavingStrategy
{
    public class OnApplicationStatusChangeSavingStrategy: ISavingStrategy, IDisposable
    {
        public event Action SaveRequested;
        
        private readonly IApplicationStatusService _applicationStatusService;

        public OnApplicationStatusChangeSavingStrategy(IApplicationStatusService applicationStatusService)
        {
            _applicationStatusService = applicationStatusService;

            _applicationStatusService.IsFocused.changed += OnFocusChanged;
            _applicationStatusService.QuitStarted += OnApplicationQuitStarted;
        }

        public void Dispose()
        {
            _applicationStatusService.IsFocused.changed -= OnFocusChanged;
            _applicationStatusService.QuitStarted -= OnApplicationQuitStarted;
        }
        
        private void OnFocusChanged(bool isFocused)
            => SaveRequested?.Invoke();
        
        private void OnApplicationQuitStarted()
            => SaveRequested?.Invoke();
    }
}