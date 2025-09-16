using System;
using Common;
using GamePlay.UI.Scripts;
using Infrastructure.DI;

namespace GamePlay.Mediators
{
    public class PauseMediator : IDisposable
    {
        private readonly PauseManager _pauseManager;
        private readonly PauseButton _pauseButton;
        private readonly PauseMenu _pauseMenu;

        public PauseMediator(IDIContainer sceneContext)
        {
            _pauseManager = sceneContext.Get<PauseManager>();
            _pauseButton = sceneContext.Get<PauseButton>();
            _pauseMenu = sceneContext.Get<PauseMenu>();

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
            if (_pauseManager.IsPaused.Value)
            {
                _pauseManager.Unlock();
                _pauseManager.Resume();
            }
            else
            {
                _pauseManager.Pause();
            }
        }
    }
}
