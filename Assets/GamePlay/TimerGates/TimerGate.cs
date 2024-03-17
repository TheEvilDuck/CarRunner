using System;
using Gameplay.UI;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class TimerGate : MonoBehaviour
    {
        public event Action<float> passed;
        private bool _passed;
        [SerializeField]private float _time;
        [SerializeField]private TimerGateView _view;

        private void Awake() 
        {
            _view.Init(_time);
        }
        private void OnTriggerEnter(Collider other) 
        {
            if (!other.gameObject.TryGetComponent<TestCar>(out TestCar testCar))
                return;

            if (_passed)
                return;

            passed?.Invoke(_time);
            _passed = true;
        }
    }
}
