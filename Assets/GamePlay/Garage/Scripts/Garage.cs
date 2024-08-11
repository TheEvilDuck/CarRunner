using System;
using Gameplay.Cars;
using UnityEngine;

namespace Gameplay.Garages
{
    [RequireComponent(typeof(Collider))]
    public class Garage : CarCollisionDetector, IGarageData
    {
        [field: SerializeField] public float TimeCost {get; private set;}
        [field: SerializeField] public CarConfig CarConfig {get; private set;}
        [SerializeField] private GarageView _garageView;
        public event Action passed;

        public void Init(GameObject wheelPrefab)
        { 
            _garageView.Init(this, wheelPrefab);
        }

        protected override void OnPassed()
        {
            passed?.Invoke();
        }
    }
}

