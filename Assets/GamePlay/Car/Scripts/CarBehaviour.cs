using System;
using UnityEngine;


public class CarBehaviour : MonoBehaviour
{
    public const float TURN_DEGREE = 30f;
    [SerializeField] private WheelData[] _wheels;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed = 500;
    private float _turnDegree = 0; //in degrees

    private void FixedUpdate()
    {
        //Turning and twisting
        foreach (WheelData wheel in _wheels)
        {
            if (wheel.IsTorque)
            {
                wheel.WheelCollider.motorTorque += _acceleration;
                if(wheel.WheelCollider.motorTorque > _maxSpeed)
                {
                    wheel.WheelCollider.motorTorque = _maxSpeed;
                }
            }
            if (wheel.IsTurnable)
            {
                wheel.WheelCollider.steerAngle = _turnDegree;
            }
        }
    }

    //turnValue - in degrees
    public void SetTurnDegree(float turnValue)
    {
        _turnDegree = turnValue;
    }

    [Serializable]
    private class WheelData
    {
        [field: SerializeField] public WheelCollider WheelCollider { get; private set; }
        [field: SerializeField] public bool IsTurnable { get; private set; }
        [field: SerializeField] public bool IsTorque { get; private set; }
    }
}