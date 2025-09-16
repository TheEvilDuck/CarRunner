using System;
using Common;
using Services.ApplicationStatus;

namespace EntryPoint.Mediators
{
    public class PauseOnApplicationFocusMediator: IDisposable
    {
        private readonly PauseManager _pauseManager;
        private readonly IApplicationStatusService _applicationStatusService;

        public PauseOnApplicationFocusMediator(PauseManager pauseManager, IApplicationStatusService applicationStatusService)
        {
            _pauseManager = pauseManager;
            _applicationStatusService = applicationStatusService;

            _applicationStatusService.IsFocused.changed += OnApplicationFocusedChanged;
        }

        public void Dispose()
        {
            _applicationStatusService.IsFocused.changed -= OnApplicationFocusedChanged;
        }
        
        private void OnApplicationFocusedChanged(bool isFocused)
        {
            if (isFocused)
                _pauseManager.Pause();
            else
                _pauseManager.Resume();
        }
    }
}