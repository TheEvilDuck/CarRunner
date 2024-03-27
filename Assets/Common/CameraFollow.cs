using UnityEngine;

namespace Common.Components
{
    public class CameraFollow : MonoBehaviour
{
    [SerializeField]private Transform _target;
    [SerializeField]private Vector3 _offset;
    [SerializeField]private float _speed = 10f;
    [SerializeField]private float _angle = 10f;
    [SerializeField]private float _minDistance = 10f;

    private float _currentSpeed;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void FixedUpdate() 
    {
        Vector3 targetPosition = _target.position+transform.rotation*_offset;

        Vector3 directionVector = targetPosition-transform.position;

        targetPosition-=Vector3.ClampMagnitude(directionVector.normalized*_minDistance,_minDistance);

        transform.position=Vector3.Lerp(transform.position,targetPosition,_speed);
        transform.rotation = _target.rotation;
        transform.Rotate(Vector3.right, _angle);

        
    }
}
}
