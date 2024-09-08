using Common;
using Common.States;
using DI;
using Gameplay.Cars;
using Gameplay.UI;
using Services.PlayerInput;
using UnityEngine;
using YG;

namespace Gameplay.States
{
    public abstract class GameOverState : State
    {
        protected readonly DIContainer _sceneContext;
        public GameOverState(StateMachine stateMachine, DIContainer sceneContext) : base(stateMachine)
        {
            _sceneContext = sceneContext;
        }

        public override void Update()
        {
            _sceneContext.Get<Car>().CarBehavior.Brake(true);

            Debug.Log("BRAKE");
        }

        protected override void OnEnter()
        {
            YandexGame.GameplayStop();

            _sceneContext.Get<Car>().CarBehavior.enabled = false;
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<PauseMenu>());
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<PauseButton>());
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<YandexGameGameplay>());
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<Car>());
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<PauseLocker>());
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<StateMachine>());
            _sceneContext.Get<PauseManager>().Unregister(_sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG));
            _sceneContext.Get<IPlayerInput>().Disable();
            _sceneContext.Get<EndOfTheGame>().Show();
            _sceneContext.Get<PauseButton>().Hide();
            
        }

        protected override void OnExit()
        {
            _sceneContext.Get<PauseManager>().Resume();
        }
    }
}