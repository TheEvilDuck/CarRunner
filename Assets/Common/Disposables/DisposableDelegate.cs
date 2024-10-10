using System;

namespace Common.Disposables
{
    public struct DisposableDelegate : IDisposable
    {
        private readonly Action _onDispose;

        public DisposableDelegate(Action onDispose) => _onDispose = onDispose;

        public void Dispose() => _onDispose?.Invoke();
    }
}
