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
        [SerializeField] private CarCollider _carCollider;
        [SerializeField, Range(0,90f)] private float _minTurnAngle = 10f;
        [SerializeField, Range(0,90f)] private float _maxTurnAngle = 55f;
        [SerializeField, Range(0,1f)] private float _turnSpeed = 0.2f;
        [SerializeField, Min(0)] private float _brakeFrictionMultiplier = 2f;
        [SerializeField, Min(0)] private float _brakePower = 2f;
        private float _acceleration;
        private float _maxSpeed;
        private float _turnDirection = 0;
        private float _startSlip;
        private bool _paused = false;
        private Vector3 _lastVelocity;
        private float _currentTurnDegree;

        public IEnumerable<IReadOnlyWheel> Wheels => _wheels;
        public float CurrentSpeed => CurrentVelocity.magnitude;
        public CarCollider CarCollider => _carCollider;
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

            float turnSpeed = Mathf.Lerp(_maxTurnAngle, _minTurnAngle, _rigidBody.velocity.magnitude / _maxSpeed) * _turnSpeed;

            if (_turnDirection == 0 && _currentTurnDegree != 0)
            {
                float angleDelta = -Mathf.Sign(_currentTurnDegree) * turnSpeed;

                if (Mathf.Sign(_currentTurnDegree + angleDelta) != MathF.Sign(_currentTurnDegree))
                    _currentTurnDegree = 0;
                else
                    _currentTurnDegree += angleDelta;
            }
            else
            {
                _currentTurnDegree += _turnDirection * turnSpeed;
            }

            _currentTurnDegree = Mathf.Clamp(_currentTurnDegree, -_maxTurnAngle, _maxTurnAngle);

            foreach (WheelData wheel in _wheels)
            {
                if (wheel.IsTorque)
                    wheel.WheelCollider.motorTorque = _acceleration;

                if (wheel.IsTurnable)
                    wheel.WheelCollider.steerAngle = _currentTurnDegree;
            }

            Vector2 currentVelocity = Vector2.ClampMagnitude(CurrentVelocity, _maxSpeed);
            _rigidBody.velocity = new Vector3(currentVelocity.x, _rigidBody.velocity.y, currentVelocity.y);
            _lastVelocity = _rigidBody.velocity;
        }

        public void Init(float Acceleration, float MaxSpeed)
        {
            _acceleration = Acceleration;
            _maxSpeed = MaxSpeed;
            _rigidBody.isKinematic = false;
        }

        public void RemoveVelocity()
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        public void SetTurnDirection(float turnValue)
        {
            _turnDirection = turnValue;
        }

        public void Brake(bool brake)
        {
            foreach (WheelData wheelData in _wheels)
                BrakeWheel(wheelData.WheelCollider, brake);
        }

        private void BrakeWheel(WheelCollider wheel, bool brake)
        {
            float brakeTorque = 0;
            float slipMultipler = 1f;

            if (brake)
            {
                brakeTorque = _acceleration * _brakePower;
                slipMultipler = _brakeFrictionMultiplier;
            }

            wheel.brakeTorque = brakeTorque;
            var friction = wheel.sidewaysFriction;  
            friction.stiffness = _startSlip * slipMultipler;
            wheel.sidewaysFriction = friction;
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