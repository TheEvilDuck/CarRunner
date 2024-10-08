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
            lastCarRotation.eulerAngles = new Vector3(0, lastCarRotation.eulerAngles.y, 0);
            _car.TeleportCar(lastCarPosition, lastCarRotation);
        }
    }
}
