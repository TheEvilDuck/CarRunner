using System;
using Common.Reactive;
using UnityEngine;

namespace Services.ApplicationStatus
{
    public class ApplicationStatusService : IApplicationStatusService, IDisposable
    {
        public event Action QuitStarted;

        private readonly Observable<bool> _isFocused = new Observable<bool>();

        public IReadonlyObservable<bool> IsFocused => _isFocused;
        
        public ApplicationStatusService()
        {
            Application.quitting += OnApplicationQuit;
            Application.focusChanged += OnFocusChanged;
        }
        
        public void Dispose()
        {
            Application.quitting -= OnApplicationQuit;
            Application.focusChanged -= OnFocusChanged;
        }

        private void OnFocusChanged(bool isFocused) => _isFocused.Value = isFocused;
        private void OnApplicationQuit() => QuitStarted?.Invoke();
    }
}