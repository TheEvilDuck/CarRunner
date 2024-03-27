using System.Collections;
using System.Collections.Generic;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public abstract class CarCollisionDetector : MonoBehaviour
    {
        private bool _passed;
        private void OnTriggerEnter(Collider other) 
        {
            if (!other.gameObject.TryGetComponent<Car>(out Car testCar))
                return;

            if (_passed)
                return;

            OnPassed();
            _passed = true;
        }

        protected abstract void OnPassed();
    }
}
