using System.Collections;
using System.Collections.Generic;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay
{
    public class CarFalling
    {
        private const float Y_POSITION_TO_TELEPORT = -30F;
        private const float MAX_GROUND_CHECK_LENGTH = 4F;
        private const float GROUND_CHECK_RATE = 1f;
        private readonly CarBehaviour _carBehaviour;
        private readonly Transform _carTransform;
        private readonly LayerMask _groundCheckLayer;
        private Quaternion _lastCarRotation;
        private Vector3 _lastRoadPosition;
        private float _lastTime;

        public CarFalling(CarBehaviour carBehaviour, Transform carTransform, LayerMask groundCheckLayer)
        {
            _carBehaviour = carBehaviour;
            _carTransform = carTransform;
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
                    _lastRoadPosition = _carTransform.position;
                    _lastCarRotation = _carTransform.rotation;
                }      

                _lastTime = Time.time;
            }

            if (CheckCarPosition())
                TeleportCar();
        }

        private bool CheckGroundFrom(Vector3 position)
        {
            return Physics.Raycast(position, Vector3.down, MAX_GROUND_CHECK_LENGTH, _groundCheckLayer);
        }

        private bool CheckCarPosition()
        {
            return _carTransform.position.y <= Y_POSITION_TO_TELEPORT + _lastRoadPosition.y;
        }

        private Vector3[] GetGroundCheckPositions()
        {
            List<Vector3> positions = new List<Vector3>();

            Vector3 center = _carBehaviour.CarCollider.Bounds.center;
            Vector3 extents =  _carBehaviour.CarCollider.Bounds.extents;

            positions.Add(center + new Vector3(extents.x, 0, extents.z));
            positions.Add(center + new Vector3(-extents.x, 0, extents.z));
            positions.Add(center + new Vector3(extents.x, 0, -extents.z));
            positions.Add(center + new Vector3(-extents.x, 0, -extents.z));

            return positions.ToArray();
        }

        private void TeleportCar()
        {
            _carTransform.position = _lastRoadPosition;
            _carTransform.rotation = _lastCarRotation;

            _carBehaviour.RemoveVelocity();
        }
    }
}
