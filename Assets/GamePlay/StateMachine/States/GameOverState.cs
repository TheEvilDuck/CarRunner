using Common.States;
using Gameplay.Cars;

namespace Gameplay.States
{
    public abstract class GameOverState : State
    {
        private readonly CarBehaviour _car;
        private readonly CarControllerMediator _carContollerMediator;
        public GameOverState(StateMachine stateMachine, CarBehaviour car, CarControllerMediator carControllerMediator) : base(stateMachine)
        {
            _car = car;
            _carContollerMediator = carControllerMediator;
        }

        public override void Update()
        {
            _car.Brake(true);
        }

        protected override void OnEnter()
        {
            _carContollerMediator.Dispose();
        }

        protected abstract void OnGameOver();
    }
}