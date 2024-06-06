using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Cars
{
    public class CarBehaviour : MonoBehaviour
    {
        private const float BREAK_COEFFICIENT = 10f;
        [SerializeField] private WheelData[] _wheels;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private float _interpolationMultiplierForRotation = 0.6f;
        private const float _maxAngleRotation = 55f;
        private float _acceleration;
        private bool _isBreak;
        private float _maxSpeed;
        private float _turnDegree = 0; //in degrees

        public IEnumerable<IReadOnlyWheel> Wheels => _wheels;

        private void FixedUpdate()
        {
            //Turning and twisting
            foreach (WheelData wheel in _wheels)
            {
                if (wheel.IsTorque)
                {
                    if (_rigidBody.velocity.magnitude < _maxSpeed)
                    {
                        wheel.WheelCollider.motorTorque = _acceleration;

                        if (_isBreak)
                            wheel.WheelCollider.brakeTorque = _acceleration * BREAK_COEFFICIENT;
                        else
                            wheel.WheelCollider.brakeTorque = 0;
                    }
                    else
                        wheel.WheelCollider.motorTorque = 0;
                }
                if (wheel.IsTurnable)
                    wheel.WheelCollider.steerAngle = Mathf.Lerp(wheel.WheelCollider.steerAngle, _turnDegree, _interpolationMultiplierForRotation);  
            }
        }

        public void Init(float Acceleration, float MaxSpeed)
        {
            _acceleration = Acceleration;
            _maxSpeed = MaxSpeed;
        }

        //_maxAngleRotation - in degrees
        public void SetTurnDirection(float turnValue)
        {
            _turnDegree = _maxAngleRotation * turnValue;
        }

        public void Brake(bool brake)
        {
            _isBreak = brake;
        }

        [Serializable]
        private class WheelData: IReadOnlyWheel
        {
            [field: SerializeField] public WheelCollider WheelCollider { get; private set; }
            [field: SerializeField] public bool IsTurnable { get; private set; }
            [field: SerializeField] public bool IsTorque { get; private set; }

            private Vector3 _worldPos;
            private Quaternion _worldRotation;

            public Vector3 WorldPosition
            {
                get
                {
                    UpdatePosAndRotation();
                    return _worldPos;
                }
            }

            public Quaternion WorldRotation 
            {
                get
                {
                    UpdatePosAndRotation();
                    return _worldRotation;
                }
            }

            private void UpdatePosAndRotation() => WheelCollider.GetWorldPose(out _worldPos, out _worldRotation);
        }
    }
}