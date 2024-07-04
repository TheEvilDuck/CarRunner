using System;
using Common;
using Gameplay.UI;
using Services.PlayerInput;

namespace Gameplay
{
    public class PauseMediator : IDisposable
    {
        private readonly PauseManager _pauseManager;
        private readonly PauseButton _pauseButton;
        private readonly PauseMenu _pauseMenu;
        private readonly IPlayerInput _playerInput;

        public PauseMediator(PauseManager pauseManager, PauseButton pauseButton, PauseMenu pauseMenu, IPlayerInput playerInput)
        {
            _pauseManager = pauseManager;
            _pauseButton = pauseButton;
            _pauseMenu = pauseMenu;
            _playerInput = playerInput;

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
                _playerInput.Enable();
            }
            else
            {
                _pauseManager.Pause();
                _pauseButton.Hide();
                _pauseMenu.Show();
                _playerInput.Disable();
            }
        }
    }
}
