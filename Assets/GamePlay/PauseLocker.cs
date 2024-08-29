using Common;

namespace Gameplay
{
    public class PauseLocker : IPausable
    {
        private readonly PauseManager _pauseManager;

        public PauseLocker(PauseManager pauseManager)
        {
            _pauseManager = pauseManager;
        }
        public void Pause() => _pauseManager.Lock();

        public void Resume() {}
    }
}
