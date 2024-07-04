using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.CarFallingHandling
{
    public class FallingTeleport : ICarFallingHandler
    {
        private readonly Car _car;
        public FallingTeleport(Car car)
        {
            _car = car;
        }
        public void HandleFalling(Vector3 lastCarPosition, Quaternion lastCarRotation)
        {
            _car.TeleportCar(lastCarPosition, lastCarRotation);
        }
    }
}
