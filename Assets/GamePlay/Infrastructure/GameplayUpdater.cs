using Common.Tickables;
using GamePlay.CarFallingHangling;

namespace GamePlay.Infrastructure
{
    public class GameplayUpdater: ITickable
    {
        private readonly Common.States.StateMachine _stateMachine;
        private readonly CarFalling _carFalling;

        public GameplayUpdater(Common.States.StateMachine stateMachine, CarFalling carFalling)
        {
            _stateMachine = stateMachine;
            _carFalling = carFalling;
        }

        public void Tick(float deltaTime)
        {
            _stateMachine.Update();
            _carFalling.Update();
        }
    }
}