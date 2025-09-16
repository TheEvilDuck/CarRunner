using System;
using Common.Settings;
using Infrastructure.DI;
using Services.ApplicationStatus;

namespace EntryPoint.Mediators
{
    public class SaveOnQuitMediator: IDisposable
    {
        private readonly ISettings _settings;
        private readonly IApplicationStatusService _applicationStatusService;

        public SaveOnQuitMediator(IDIContainer container)
        {
            _applicationStatusService = container.Get<IApplicationStatusService>();
            _settings = container.Get<ISettings>();

            _applicationStatusService.QuitStarted += OnQuiStarted;
            _applicationStatusService.IsFocused.changed += OnFocusChanged;
        }

        public void Dispose()
        {
            _applicationStatusService.QuitStarted -= OnQuiStarted;
            _applicationStatusService.IsFocused.changed -= OnFocusChanged;
        }
        
        private void OnFocusChanged(bool obj)
        {
            if (obj == false)
                Save();
        }

        private void OnQuiStarted() => Save();

        private void Save()
        {
            _settings.SaveSettings();
        }
    }
}