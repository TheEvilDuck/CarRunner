using System;
using Common;
using Gameplay.UI;

namespace Gameplay
{
    public class PauseMediator : IDisposable
    {
        private readonly PauseManager _pauseManager;
        private readonly PauseButton _pauseButton;
        private readonly PauseMenu _pauseMenu;

        public PauseMediator(PauseManager pauseManager, PauseButton pauseButton, PauseMenu pauseMenu)
        {
            _pauseManager = pauseManager;
            _pauseButton = pauseButton;
            _pauseMenu = pauseMenu;

            _pauseButton.pressed += OnPauseButtonPressed;
            _pauseMenu.ResumeButtonPressed.AddListener(OnPauseButtonPressed);
        }
        public void Dispose()
        {
            _pauseButton.pressed -= OnPauseButtonPressed;
            _pauseMenu.ResumeButtonPressed.RemoveListener(OnPauseButtonPressed);
        }

        private void OnPauseButtonPressed()
        {
            if (_pauseManager.Paused)
            {
                _pauseManager.Resume();
                _pauseButton.Show();
                _pauseMenu.Hide();
            }
            else
            {
                _pauseManager.Pause();
                _pauseButton.Hide();
                _pauseMenu.Show();
            }
        }
    }
}
