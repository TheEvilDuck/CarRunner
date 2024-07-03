using System;
using System.Collections.Generic;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.CarFallingHandling
{
    public class CarFalling
    {
        public event Action<Vector3, Quaternion> carFallen;
        private const float Y_POSITION_TO_TELEPORT = -30F;
        private const float MAX_GROUND_CHECK_LENGTH = 4F;
        private const float GROUND_CHECK_RATE = 1f;
        private readonly Car _car;
        private readonly LayerMask _groundCheckLayer;
        private Quaternion _lastCarRotation;
        private Vector3 _lastRoadPosition;
        private float _lastTime;

        public CarFalling(Car car, LayerMask groundCheckLayer)
        {
            _car = car;
            _groundCheckLayer = groundCheckLayer;
        }

        public void Update()
        {
            if (Time.time - _lastTime >= GROUND_CHECK_RATE)
            {
                bool grounded = true;

                foreach (Vector3 groundCheckPosition in GetGroundCheckPositions())
                {
                    if (!CheckGroundFrom(groundCheckPosition))
                    {
                        grounded = false;
                        break;
                    }
                }

                if (grounded)
                {
                    _lastRoadPosition = _car.Position - new Vector3(_car.CarBehavior.CurrentVelocity.x, 0, _car.CarBehavior.CurrentVelocity.y) * Time.deltaTime;
                    _lastCarRotation = _car.Rotation;
                }      

                _lastTime = Time.time;
            }

            if (CheckCarPosition())
                carFallen?.Invoke(_lastRoadPosition, _lastCarRotation);
        }

        private bool CheckGroundFrom(Vector3 position)
        {
            return Physics.Raycast(position, Vector3.down, MAX_GROUND_CHECK_LENGTH, _groundCheckLayer);
        }

        private bool CheckCarPosition()
        {
            return _car.Position.y <= Y_POSITION_TO_TELEPORT + _lastRoadPosition.y;
        }

        private Vector3[] GetGroundCheckPositions()
        {
            List<Vector3> positions = new List<Vector3>();

            Vector3 center = _car.CarBehavior.CarCollider.Bounds.center;
            Vector3 extents =  _car.CarBehavior.CarCollider.Bounds.extents;

            positions.Add(center + new Vector3(extents.x, 0, extents.z));
            positions.Add(center + new Vector3(-extents.x, 0, extents.z));
            positions.Add(center + new Vector3(extents.x, 0, -extents.z));
            positions.Add(center + new Vector3(-extents.x, 0, -extents.z));

            return positions.ToArray();
        }
    }
}
