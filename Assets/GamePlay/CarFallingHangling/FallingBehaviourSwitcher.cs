using System;

namespace Gameplay.CarFallingHandling
{
    public class FallingBehaviourSwitcher: IDisposable
    {
        private readonly CarFalling _carFalling;
        private ICarFallingHandler _carFallingHandler;

        public FallingBehaviourSwitcher(CarFalling carFalling)
        {
            _carFalling = carFalling;
        }

        public void AttachBehaviour(ICarFallingHandler carFallingHandler)
        {
            if (_carFallingHandler != null)
                _carFalling.carFallen -= _carFallingHandler.HandleFalling;

            _carFallingHandler = carFallingHandler;
            _carFalling.carFallen += _carFallingHandler.HandleFalling;
        }

        public void Dispose()
        {
            if (_carFallingHandler != null)
                _carFalling.carFallen -= _carFallingHandler.HandleFalling;
        }
    }
}
