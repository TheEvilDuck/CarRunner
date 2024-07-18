using System;
using System.Collections.Generic;

namespace Common
{
    public class PauseManager: IPausable
    {
        private List<IPausable> _pausables;

        public bool Paused {get; private set;}

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
            if (Paused == false)
            {
                foreach (IPausable pausable in _pausables)
                {
                    pausable.Pause();
                }
            }

            Paused = true;
        }

        public void Resume()
        {
            if(Paused == true)
            {
                foreach (IPausable pausable in _pausables)
                {
                    pausable.Resume();
                }
            }

            Paused = false;
        }
    }
}
