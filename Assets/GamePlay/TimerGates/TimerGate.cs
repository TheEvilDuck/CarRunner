using System;
using Gameplay.UI;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class TimerGate : CarCollisionDetector
    {
        [SerializeField]private float _time;
        [SerializeField]private TimerGateView _view;
        public event Action<float> passed;

        private void Awake() 
        {
            _view.Init(_time);
        }

        protected override void OnPassed()
        {
            passed?.Invoke(_time);
        }
    }
}
