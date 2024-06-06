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
        private const float _maxAngleRotation = 35f;
        private float _acceleration;
        private bool _isBreak;
        private float _maxSpeed;
        private float _turnDegree = 0; //in degrees
        private float _interpolationMultiplierForRotation = 0.6f;
        private bool _stopped;

        public IEnumerable<IReadOnlyWheel> Wheels => _wheels;

        private void FixedUpdate()
        {
            if (_stopped)
            {
                _rigidBody.velocity *= 0.9f;

                if (_rigidBody.velocity.magnitude < 0.01f)
                    _rigidBody.velocity = Vector3.zero;

                return;
            }
            
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

        public void Stop()
        {
            foreach (var wheel in _wheels)
            {
                wheel.WheelCollider.motorTorque = 0;
                wheel.WheelCollider.steerAngle = 0;
            }

            _stopped = true;
        }

        //_maxAngleRotation - in degrees
        public void SetTurnDegree(float turnValue)
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