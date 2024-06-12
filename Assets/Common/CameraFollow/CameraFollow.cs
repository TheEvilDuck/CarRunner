using UnityEngine;

namespace Common.Components
{
    public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _lookAtOffset;
    [SerializeField, Range(0, 1f)] private float _speed;
    

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void Update() 
    {
        if (_target == null)
            return;

        var targetPosition = _target.position + _target.TransformDirection(_offset);
        var moveVector = targetPosition - transform.position;

        var interpolated = Vector3.Lerp(transform.position, targetPosition, _speed * moveVector.magnitude * Time.deltaTime);
        transform.position = interpolated;
        
        transform.LookAt(_target.position + _target.TransformDirection(_lookAtOffset));
        
        
    }
}
}
