using System;
using UnityEngine;

namespace Gameplay.Cars
{
    [RequireComponent(typeof(Collider))]
    public class CarCollider : MonoBehaviour
    {
        public Action collided;
        private Collider _collider;

        public Bounds Bounds => _collider.bounds;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }
        private void OnCollisionEnter(Collision other) 
        {
            collided?.Invoke();
        }
    }
}
