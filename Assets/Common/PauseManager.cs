using System;
using System.Collections.Generic;
using Common.Reactive;
using UnityEngine;

namespace Common
{
    public class PauseManager: IPausable
    {
        private List<IPausable> _pausables;

        private Observable<bool> _isPaused;
        private bool _locked = false;

        public IReadonlyObservable<bool> IsPaused => _isPaused;

        public PauseManager()
        {
            _isPaused = new Observable<bool>();
            _pausables = new List<IPausable>();
        }

        public void Lock() => _locked = true;
        public void Unlock() => _locked = false;

        public void Register(IPausable pausable)
        {
            if (pausable == this)
                throw new ArgumentException("Pause manager must not contain itself");

            if (_pausables.Contains(pausable))
            {
                Debug.LogWarning($"Pausable {pausable.GetType()} is already registered! Registration will be ignored");
            }

            _pausables.Add(pausable);
        }

        public void Unregister(IPausable pausable)
        {
            if (pausable == this)
                throw new ArgumentException("Pause manager must not contain itself");

            if (!_pausables.Remove(pausable))
                throw new ArgumentException($"This pause mabanger doesn't contain passed pausable of type {pausable.GetType()}");
        }

        public void Pause()
        {
            if (_locked)
                return;

            List<IPausable> markedToDelete = new List<IPausable>();

            if (_isPaused.Value == false)
            {
                foreach (IPausable pausable in _pausables)
                {
                    if (pausable == null)
                        markedToDelete.Add(pausable);
                    else
                        pausable.Pause();
                }
            }

            foreach (var pausable in markedToDelete)
                _pausables.Remove(pausable);

            _isPaused.Value = true;
        }

        public void Resume()
        {
            if (_locked)
                return;

            List<IPausable> markedToDelete = new List<IPausable>();

            if(_isPaused.Value == true)
            {
                foreach (IPausable pausable in _pausables)
                {
                    if (pausable == null)
                        markedToDelete.Add(pausable);
                    else
                        pausable.Resume();
                }
            }

            foreach (var pausable in markedToDelete)
                _pausables.Remove(pausable);

            _isPaused.Value = false;
        }
    }
}
