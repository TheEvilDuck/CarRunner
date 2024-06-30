using System.Collections;
using System.Collections.Generic;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay
{
    public class CarFalling
    {
        private const float Y_POSITION_TO_TELEPORT = -30F;
        private readonly CarBehaviour _carBehaviour;
        private readonly Transform _carTransform;
        private Vector3 _carStartPosition;
        private Quaternion _carStartRotation;

        public CarFalling(CarBehaviour carBehaviour, Transform carTransform)
        {
            _carBehaviour = carBehaviour;
            _carTransform = carTransform;

            _carStartPosition = _carTransform.position;
            _carStartRotation = _carTransform.rotation;
        }

        public void Update()
        {
            if (CheckCarPosition())
                TeleportCar();
        }

        private bool CheckCarPosition()
        {
            return _carTransform.position.y <= Y_POSITION_TO_TELEPORT;
        }

        private void TeleportCar()
        {
            _carTransform.position = _carStartPosition;
            _carTransform.rotation = _carStartRotation;

            _carBehaviour.RemoveVelocity();
        }
    }
}
