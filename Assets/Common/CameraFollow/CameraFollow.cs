using UnityEngine;

namespace Common.Components
{
    public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField, Range(0,1f)] private float _speed = 0.75f;
    [SerializeField]private float _angle = 10f;
    [SerializeField, Min(0)] private float _minDistance = 10f;
    [SerializeField, Min(0)] private float _minTargetDistanceDelta;
    [SerializeField, Min(0)] private float _minTargetAngleDelta;

    private Vector3 _lastTargetPosition;
    private Quaternion _lastTargetRotation;
    

    public void SetTarget(Transform target)
    {
        _target = target;
        _lastTargetPosition = _target.position;
    }

    private void FixedUpdate() 
    {
        if (_target == null)
        {
            return;
        }

        if ((_lastTargetPosition - _target.position).magnitude >= _minTargetDistanceDelta)
            _lastTargetPosition = _target.position;

        if (Quaternion.Angle(_lastTargetRotation, _target.rotation) >= _minTargetAngleDelta)
            _lastTargetRotation = _target.rotation;

        Vector3 targetPosition = _lastTargetPosition + transform.rotation * _offset;

        Vector3 directionVector = targetPosition - transform.position;
        var speed = directionVector.magnitude * _speed;
        var position = Vector3.Lerp(transform.position, targetPosition, _speed);
        transform.position = position;

        transform.rotation = _target.rotation;
        transform.Rotate(Vector3.right, _angle);
    }
}
}
