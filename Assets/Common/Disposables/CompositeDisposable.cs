using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Disposables
{
    public class CompositeDisposable : IDisposable
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public void Add(IDisposable disposable)
        {
            if (_disposables.Contains(disposable))
            {
                Debug.LogWarning($"Disposable {disposable} already exists in this disposable container and will be ignored");
                return;
            }

            _disposables.Add(disposable);
        }

        public void Remove(IDisposable disposable) => _disposables.Remove(disposable);

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable?.Dispose();

            _disposables.Clear();
        }
    }
}
