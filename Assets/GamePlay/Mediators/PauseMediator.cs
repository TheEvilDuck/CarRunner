using System;
using Common;
using Gameplay.UI;

namespace Gameplay
{
    public class PauseMediator : IDisposable
    {
        private readonly PauseManager _pauseManager;
        private readonly PauseButton _pauseButton;

        public PauseMediator(PauseManager pauseManager, PauseButton pauseButton)
        {
            _pauseManager = pauseManager;
            _pauseButton = pauseButton;

            _pauseButton.pressed += OnPauseButtonPressed;
        }
        public void Dispose()
        {
            _pauseButton.pressed -= OnPauseButtonPressed;
        }

        private void OnPauseButtonPressed()
        {
            _pauseManager.Pause();
        }
    }
}
