using Common;
using System;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuMediator : IDisposable
    {
        private MainMenuView _mainMenuView;
        private SceneLoader _sceneLoader;

        public MainMenuMediator(MainMenuView mainMenuView, SceneLoader sceneLoader)
        {
            _mainMenuView = mainMenuView;
            _sceneLoader = sceneLoader;

            _mainMenuView.ExitClickedEvent.AddListener(OnExitClecked);
            _mainMenuView.PlayClickedEvent.AddListener(OnPlayClecked); 
        }

        public void Dispose()
        {
            _mainMenuView.ExitClickedEvent.RemoveListener(OnExitClecked);
            _mainMenuView.PlayClickedEvent.RemoveListener(OnPlayClecked);
        }

        private void OnExitClecked() => Application.Quit();
        private void OnPlayClecked() => _sceneLoader.LoadGameplay();
    }
}

