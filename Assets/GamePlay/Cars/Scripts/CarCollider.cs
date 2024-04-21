using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Cars
{
    [RequireComponent(typeof(Collider))]
    public class CarCollider : MonoBehaviour
    {
        public Action collided;
        private void OnCollisionEnter(Collision other) 
        {
            collided?.Invoke();
        }
    }
}
