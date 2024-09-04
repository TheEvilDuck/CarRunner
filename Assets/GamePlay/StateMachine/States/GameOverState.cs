using Common;
using Common.States;
using DI;
using Gameplay.Cars;
using Gameplay.UI;
using Services.PlayerInput;

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
        }

        protected override void OnEnter()
        {
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<PauseMenu>());
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<PauseButton>());
            _sceneContext.Get<IPlayerInput>().Disable();
            _sceneContext.Get<EndOfTheGame>().Show();
            _sceneContext.Get<PauseButton>().Hide();
            _sceneContext.Get<PauseManager>().Pause();
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<Car>());
        }

        protected override void OnExit()
        {
            _sceneContext.Get<PauseManager>(Gameplay.Bootstrap.GAMEPLAY_PAUSE_MANAGER_TAG).Unregister(_sceneContext.Get<PauseLocker>());
            _sceneContext.Get<PauseManager>().Resume();
        }
    }
}