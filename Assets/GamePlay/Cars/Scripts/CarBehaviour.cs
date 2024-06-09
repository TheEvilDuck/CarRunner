using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Gameplay.Cars
{
    public class CarBehaviour : MonoBehaviour, IPausable
    {
        [SerializeField] private WheelData[] _wheels;
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField, Range(0,90f)] private float _minTurnAngle = 10f;
        [SerializeField, Range(0,90f)] private float _maxTurnAngle = 55f;
        [SerializeField, Min(0)] private float _brakeFrictionMultiplier = 2f;
        [SerializeField, Min(0)] private float _brakePower = 2f;
        private float _acceleration;
        private bool _isBrake;
        private float _maxSpeed;
        private float _turnDirection;
        private float _startSlip;
        private bool _paused = false;
        private Vector3 _lastVelocity;

        public IEnumerable<IReadOnlyWheel> Wheels => _wheels;
        public float CurrentSpeed => CurrentVelocity.magnitude;
        private Vector2 CurrentVelocity => new Vector2(_rigidBody.velocity.x, _rigidBody.velocity.z);

        private void OnValidate() 
        {
            _maxTurnAngle = Mathf.Max(_minTurnAngle, _maxTurnAngle);
        }

        private void Awake() 
        {
            foreach (WheelData wheel in _wheels)
            {
                _startSlip = wheel.WheelCollider.sidewaysFriction.stiffness;
            }
        }
        

        private void FixedUpdate()
        {
            if (_paused)
                return;

            foreach (WheelData wheel in _wheels)
            {
                if (wheel.IsTorque)
                    wheel.WheelCollider.motorTorque = _acceleration;

                var currentVelocity = Vector2.ClampMagnitude(CurrentVelocity, _maxSpeed);
                _rigidBody.velocity = new Vector3(currentVelocity.x, _rigidBody.velocity.y, currentVelocity.y);

                if (wheel.IsTurnable)
                    wheel.WheelCollider.steerAngle = _turnDirection * Mathf.Lerp(_maxTurnAngle, _minTurnAngle, currentVelocity.magnitude / _maxSpeed);

                
                float brakeTorque = 0;
                float slipMultipler = 1f;

                if (_isBrake)
                {
                    brakeTorque = _acceleration * _brakePower;
                    slipMultipler = _brakeFrictionMultiplier;
                }

                wheel.WheelCollider.brakeTorque = brakeTorque;
                var friction = wheel.WheelCollider.sidewaysFriction;
                friction.stiffness = _startSlip * slipMultipler;
                wheel.WheelCollider.sidewaysFriction = friction;
                
            }

            _lastVelocity = _rigidBody.velocity;
        }

        public void Init(float Acceleration, float MaxSpeed)
        {
            _acceleration = Acceleration;
            _maxSpeed = MaxSpeed;
            _rigidBody.isKinematic = false;
        }

        //_maxAngleRotation - in degrees
        public void SetTurnDirection(float turnValue)
        {
            _turnDirection = turnValue;
        }

        public void Brake(bool brake)
        {
            _isBrake = brake;
        }

        public void Pause()
        {
            _lastVelocity = _rigidBody.velocity;

            _paused = true;
            _rigidBody.freezeRotation = true;
            _rigidBody.isKinematic = true;
        }

        public void Resume()
        {
            _rigidBody.freezeRotation = false;
            _rigidBody.isKinematic = false;

            _rigidBody.velocity = _lastVelocity;

            _paused = false;
        }

        [Serializable]
        private class WheelData: IReadOnlyWheel
        {
            [field: SerializeField] public WheelCollider WheelCollider { get; private set; }
            [field: SerializeField] public bool IsTurnable { get; private set; }
            [field: SerializeField] public bool IsTorque { get; private set; }
            public float Radius => WheelCollider.radius;


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