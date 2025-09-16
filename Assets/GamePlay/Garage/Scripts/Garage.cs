using System;
using Common.Reactive;
using GamePlay.Cars.Scripts;
using UnityEngine;

namespace GamePlay.Garage.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class Garage : CarCollisionDetector, IGarageData
    {
        [field: SerializeField] public float TimeCost {get; private set;}
        [field: SerializeField] public CarConfig CarConfig {get; private set;}
        [SerializeField] private GarageView _garageView;
        public event Action passed;

        public void Init(GameObject wheelPrefab, IReadonlyObservable<CarConfig> currentCarConfig)
        { 
            _garageView.Init(this, wheelPrefab, currentCarConfig);
        }

        protected override void OnPassed()
        {
            passed?.Invoke();
        }
    }
}

