using System;
using Common.Reactive;

namespace Services.ApplicationStatus
{
    public interface IApplicationStatusService
    {
        event Action QuitStarted;
        IReadonlyObservable<bool> IsFocused { get; }
    }
}