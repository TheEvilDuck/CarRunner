using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCar : MonoBehaviour
{
    private Vector2 _moveVector;
    [SerializeField]private float _speed;
    [SerializeField]private Rigidbody _rigidBody;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _moveVector = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate() 
    {
        _rigidBody.velocity = new Vector3(_moveVector.x*_speed, _rigidBody.velocity.y, _moveVector.y*_speed);
    }
}
