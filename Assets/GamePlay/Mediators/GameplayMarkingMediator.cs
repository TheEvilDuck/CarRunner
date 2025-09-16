using System;
using GamePlay.StateMachine.States;
using Infrastructure.DI;
using Services.Integrations;
using Services.SceneManagement;

namespace GamePlay.Mediators
{
    public class GameplayMarkingMediator: IDisposable
    {
        private readonly ISceneManager _sceneManager;
        private readonly YandexGameIntegrator _yandexGameIntegrator;
        private readonly Common.States.StateMachine _gameplayStateMachine;

        public GameplayMarkingMediator(IDIContainer container)
        {
            _sceneManager = container.Get<ISceneManager>();
            _yandexGameIntegrator = container.Get<YandexGameIntegrator>();
            _gameplayStateMachine = container.Get<Common.States.StateMachine>();

            _sceneManager.sceneLoadingRequested += OnSceneLoadingRequested;

            _gameplayStateMachine.GetState<WinState>().entered += OnWinStateEntered;
            _gameplayStateMachine.GetState<LoseState>().entered += OnLoseStateEntered;
        }

        public void Dispose()
        {
            _sceneManager.sceneLoadingRequested -= OnSceneLoadingRequested;
            _gameplayStateMachine.GetState<WinState>().entered -= OnWinStateEntered;
            _gameplayStateMachine.GetState<LoseState>().entered -= OnLoseStateEntered;
        }
        private void OnSceneLoadingRequested(string sceneName) => _yandexGameIntegrator.MarkGameplay(false);
        private void OnLoseStateEntered() => _yandexGameIntegrator.MarkGameplay(false);
        private void OnWinStateEntered() => _yandexGameIntegrator.MarkGameplay(false);
    }
}