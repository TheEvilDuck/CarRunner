using Common.States;
using Gameplay.Cars;

namespace Gameplay.States
{
    public abstract class GameOverState : State
    {
        private readonly CarBehaviour _car;
        public GameOverState(StateMachine stateMachine, CarBehaviour car) : base(stateMachine)
        {
            _car = car;
        }

        public override void Update()
        {
            _car.Brake(true);
        }

        protected abstract void OnGameOver();
    }
}