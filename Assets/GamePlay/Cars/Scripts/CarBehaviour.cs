using System;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay.Cars
{
    public class CarBehaviour : MonoBehaviour
    {
        //public const float TURN_DEGREE = 30f;
        private const float _innerTurnDegree = 45f;
        private float _externalTurnDegree = (float)(Math.Atan2(2.37f, 1.3f + 2.37f / Math.Tan(_innerTurnDegree))*180/Math.PI);
        [SerializeField] private WheelData[] _wheels;
        [SerializeField] private Rigidbody _rigidBody;
        private float _acceleration;
        private float _maxSpeed = 500;
        private float _turnDegree = 0; //in degrees

        public IEnumerable<IReadOnlyWheel> Wheels => _wheels;

        private void FixedUpdate()
        {
            //Turning and twisting
            foreach (WheelData wheel in _wheels)
            {
                if (wheel.IsTorque)
                {
                    
                    if(_rigidBody.velocity.magnitude < _maxSpeed)
                    {
                        wheel.WheelCollider.motorTorque = _acceleration;
                    }
                    else
                    {
                        wheel.WheelCollider.motorTorque = 0;
                    }
                }
                if (wheel.IsTurnable)
                {
                    if(_turnDegree > 0)
                    {
                        if (!wheel.IsLeft)
                        {
                            wheel.WheelCollider.steerAngle = _turnDegree;
                        }
                        if (wheel.IsLeft)
                        {
                            wheel.WheelCollider.steerAngle = _externalTurnDegree;
                        }
                    }
                    if (_turnDegree < 0)
                    {
                        if (wheel.IsLeft)
                        {
                            wheel.WheelCollider.steerAngle = _turnDegree;
                        }
                        if (!wheel.IsLeft)
                        {
                            wheel.WheelCollider.steerAngle = -_externalTurnDegree;
                        }
                    }
                    if(_turnDegree == 0)
                    {
                        wheel.WheelCollider.steerAngle = 0;
                    }
                }
                
            }
        }

        public void Init(float Acceleration, float MaxSpeed)
        {
            _acceleration = Acceleration;
            _maxSpeed = MaxSpeed;
        }

        //turnValue - in degrees
        public void SetTurnDegree(float turnValue)
        {
            _turnDegree = _innerTurnDegree * turnValue;
        }

        [Serializable]
        private class WheelData: IReadOnlyWheel
        {
            [field: SerializeField] public WheelCollider WheelCollider { get; private set; }
            [field: SerializeField] public bool IsTurnable { get; private set; }
            [field: SerializeField] public bool IsTorque { get; private set; }
            [field: SerializeField] public bool IsLeft {  get; private set; }

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