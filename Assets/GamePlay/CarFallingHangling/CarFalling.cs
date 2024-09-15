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
        private const float GROUND_CHECK_RATE = 0.5f;
        private const float GROUND_CHECK_OFFSET = 6f;
        private const float Y_POSITION_OFFSET = 3f;
        private const float FALL_DEBOUNCE_TIME = 1f;
        private const float MAX_ANGLE_GROUNDED = 35f;
        private readonly Car _car;
        private readonly LayerMask _groundCheckLayer;
        private Quaternion _lastCarRotation;
        private Vector3 _lastRoadPosition;
        private float _lastTime;
        private float _returnLastTime;

        public CarFalling(Car car, LayerMask groundCheckLayer)
        {
            _car = car;
            _groundCheckLayer = groundCheckLayer;

            _lastCarRotation = car.Rotation;
            _lastRoadPosition = car.Position;
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
                    _lastRoadPosition = _car.Position;
                    _lastCarRotation = _car.Rotation;
                }      

                _lastTime = Time.time;

                
            }

            if (CheckCarPosition())
            {
                if (Time.time - _returnLastTime >= FALL_DEBOUNCE_TIME)
                {
                    carFallen?.Invoke(_lastRoadPosition + Vector3.up * Y_POSITION_OFFSET, _lastCarRotation);
                    _returnLastTime = Time.time;
                } 
            }
            
        }

        private bool CheckGroundFrom(Vector3 position)
        {
            if (Physics.Raycast(position, Vector3.down, out var result, MAX_GROUND_CHECK_LENGTH, _groundCheckLayer))
            {
                if (Vector3.Angle(result.normal, Vector3.up) <= MAX_ANGLE_GROUNDED)
                    return true;
            }

            return false;
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

            positions.Add(GetOffsetedColliderPosition(center, new Vector3(extents.x, 0, extents.z)));
            positions.Add(GetOffsetedColliderPosition(center, new Vector3(-extents.x, 0, extents.z)));
            positions.Add(GetOffsetedColliderPosition(center, new Vector3(extents.x, 0, -extents.z)));
            positions.Add(GetOffsetedColliderPosition(center, new Vector3(-extents.x, 0, -extents.z)));

            return positions.ToArray();
        }

        private Vector3 GetOffsetedColliderPosition(Vector3 center, Vector3 extents)
        {
            Vector3 corner = center + extents;
            Vector3 offset = extents.normalized * GROUND_CHECK_OFFSET;
            return corner + offset;
        }
    }
}
