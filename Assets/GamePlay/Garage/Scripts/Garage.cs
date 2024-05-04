using System;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.Garages
{
    [RequireComponent(typeof(Collider))]
    public class Garage : CarCollisionDetector, IGarageData
    {
        [field: SerializeField] public float AdditionalTime {get; private set;}
        [field: SerializeField] public CarConfig CarConfig {get; private set;}
        [field: SerializeField] public float ComparsionTime {get; private set;}
        [SerializeField] private GarageView _garageView;
        private Timer _timer; 
        public event Action<bool> passed;

        public void Init(Timer timer)
        {
            _timer = timer;     

            _garageView.Init(this);
        }

        protected override void OnPassed()
        {
            passed?.Invoke(_timer.CurrentTime >= ComparsionTime);
        }
    }
}

