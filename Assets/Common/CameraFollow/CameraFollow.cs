using UnityEngine;

namespace Common.Components
{
    public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _lookAtOffset;
    [SerializeField, Min(0)] private float _minTargetVelocity;
    [SerializeField, Min(0)] private float _minVelocityAngle;
    [SerializeField, Range(0, 1f)] private float _speed;

    private Vector3 _targetLastPosition;
    private Vector3 _lastVelocity;
    

    public void SetTarget(Transform target)
    {
        _target = target;
        _targetLastPosition = _target.position;
    }

    private void Update() 
    {
        if (_target == null)
            return;

        Vector3 moveVector = _target.position - _targetLastPosition;
        float angle = Vector3.SignedAngle(moveVector.normalized, Vector3.forward, Vector3.up);

        Vector3 offset = Quaternion.AngleAxis(-angle, Vector3.up) * _offset;

        if (moveVector.sqrMagnitude < Mathf.Pow(_minTargetVelocity, 2) || Mathf.Abs(angle) < _minVelocityAngle)
        {
            angle = Vector3.SignedAngle(_lastVelocity.normalized, Vector3.forward, Vector3.up);
            offset = Quaternion.AngleAxis(-angle, Vector3.up) * _offset;
        }
        else
        {
            _lastVelocity = moveVector;
        }

        Vector3 goal =  _target.position + offset;
        float speed = (goal - transform.position).magnitude * _speed * Time.deltaTime;
        Vector3 goalLerp = Vector3.LerpUnclamped(transform.position, goal, speed);

        transform.position = goalLerp;
        transform.LookAt(_target.position);
        
        _targetLastPosition = _target.position;
    }
}
}
