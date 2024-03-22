using System;
using Services.PlayerInput;

namespace Gameplay
{
    public class CarControllerMediator : IDisposable
    {
        private readonly CarBehaviour _car;
        private readonly IPlayerInput _playerInput;

        public CarControllerMediator(CarBehaviour carBehaviour, IPlayerInput playerInput)
        {
            _car = carBehaviour;
            _playerInput = playerInput;

            _playerInput.horizontalInput+=OnHorizontalInput;
        }
        public void Dispose()
        {
            _playerInput.horizontalInput-=OnHorizontalInput;
        }

        //сюда прилетают значения от -1 до 1
        private void OnHorizontalInput(float horizontalInput)
        {
            _car.SetTurnDegree(horizontalInput);
        }
    }
}

    
