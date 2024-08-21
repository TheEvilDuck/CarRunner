using Common.Sound;
using Common.States;
using DI;
using Gameplay.Cars;
using Gameplay.UI;
using Levels;
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
            _sceneContext.Get<IPlayerInput>().Disable();
            _sceneContext.Get<EndOfTheGame>().Show();
            _sceneContext.Get<PauseButton>().Hide();
        }

        protected override void OnExit()
        {
            _sceneContext.Get<SoundController>().Stop(_sceneContext.Get<Level>().BackGroundMusicId);
        }
    }
}