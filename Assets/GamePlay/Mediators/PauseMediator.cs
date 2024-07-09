using System;
using Common;
using DI;
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

        public PauseMediator(DIContainer sceneContext)
        {
            _pauseManager = sceneContext.Get<PauseManager>();
            _pauseButton = sceneContext.Get<PauseButton>();
            _pauseMenu = sceneContext.Get<PauseMenu>();
            _playerInput = sceneContext.Get<IPlayerInput>();

            _pauseButton.pressed += OnPauseButtonPressed;
            _pauseMenu.ResumeButtonPressed.AddListener(OnPauseButtonPressed);

            _pauseMenu.Hide();
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
