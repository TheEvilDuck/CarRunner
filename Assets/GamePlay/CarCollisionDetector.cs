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
            if (!other.gameObject.TryGetComponent<CarCollider>(out CarCollider carCollider))
                return;

            if (_passed)
                return;

            OnPassed();
            _passed = true;
        }

        protected abstract void OnPassed();
    }
}
