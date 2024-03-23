using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class Garage : CarCollisionDetector
    {
        [SerializeField] private float _time;
        private Timer _timer; 
        public event Action<bool> passed;

        public void Init(Timer timer)
        {
            _timer = timer;       
        }

        protected override void OnPassed()
        {
            passed?.Invoke(_timer.CurrentTime >= _time);
        }
    }
}

