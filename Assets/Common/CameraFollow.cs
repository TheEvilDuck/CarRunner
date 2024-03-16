using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]private Transform _target;
    [SerializeField]private Vector3 _offset;
    [SerializeField]private float _midDistance;
    [SerializeField]private float _acceleration;
    [SerializeField]private float _angle;

    private float _currentSpeed;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Update() 
    {
        Vector3 targetPosition = _target.position+transform.rotation*_offset;

        Vector3 directionVector = targetPosition-transform.position;

        

        if (directionVector.magnitude<=_midDistance)
            if (_currentSpeed>0)
                _currentSpeed-=0.2f*Time.deltaTime;
            else
                _currentSpeed = 0;
        else
            _currentSpeed = _acceleration*Time.deltaTime;


        transform.position+=directionVector*_currentSpeed;
        transform.rotation = _target.rotation;
        transform.Rotate(Vector3.right, _angle);

        
    }
}
