using System;
using UnityEngine;

namespace Gameplay.TimerGates
{
    [RequireComponent(typeof(Collider))]
    public class TimerGate : CarCollisionDetector
    {
        [SerializeField]private float _time;
        [SerializeField]private TimerGateView _view;

        public float Time => _time;
        public event Action<float> passed;

        private void Awake() 
        {
            _view.Init(this);
        }

        protected override void OnPassed()
        {
            passed?.Invoke(_time);
        }
    }
}
