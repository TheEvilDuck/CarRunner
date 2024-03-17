using System;
using UnityEngine;


public class CarBehaviour : MonoBehaviour
{
    [SerializeField] private WheelData[] _wheels;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _maxSpeed = 2500;
    private float _turnDegree; //в градусах

    private void FixedUpdate()
    {
        //поворот колеса 
        //езда тоже через колеса
        foreach(WheelData wheel in _wheels)
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

    //turnValue - в градусах
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