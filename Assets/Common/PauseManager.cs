using System;
using System.Collections.Generic;

namespace Common
{
    public class PauseManager: IPausable
    {
        private List<IPausable> _pausables;

        public PauseManager()
        {
            _pausables = new List<IPausable>();
        }

        public void Register(IPausable pausable)
        {
            if (pausable == this)
                throw new ArgumentException("Pause manager must not contain itself");

            _pausables.Add(pausable);
        }

        public void Pause()
        {
            foreach (IPausable pausable in _pausables)
            {
                pausable.Pause();
            }
        }

        public void Resume()
        {
            foreach (IPausable pausable in _pausables)
            {
                pausable.Resume();
            }
        }
    }
}
