using System;
using GamePlay.Cars.Scripts;
using Infrastructure.DI;
using Services.InputService;

namespace GamePlay.Mediators
{
    public class CarControllerMediator : IDisposable
    {
        private readonly CarBehaviour _car;
        private readonly PlayerInput _playerInput;

        public CarControllerMediator(IDIContainer sceneContext)
        {
            _car = sceneContext.Get<Car>().CarBehavior;
            _playerInput = sceneContext.Get<PlayerInput>();

            _playerInput.horizontalInput += OnHorizontalInput;
            _playerInput.brakeInput += OnBrakeInput;
        }
        public void Dispose()
        {
            _playerInput.horizontalInput -= OnHorizontalInput;
            _playerInput.brakeInput -= OnBrakeInput;
        }

        //сюда прилетают значения от -1 до 1
        private void OnHorizontalInput(float horizontalInput)
        {
            _car.SetTurnDirection(horizontalInput);
        }

        private void OnBrakeInput(bool isBraking)
        {
            _car.Brake(isBraking);
        }
    }
}

    
