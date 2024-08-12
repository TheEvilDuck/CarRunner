using System;
using System.Collections.Generic;
using Common.Reactive;
using Gameplay.Cars;
using Gameplay.Garages;
using UnityEngine;

namespace Gameplay
{
    public class CarSwitcher: IDisposable
    {
        private Car _car;
        private IEnumerable<IGarageData> _garages;
        private Timer _timer;

        private Dictionary<IGarageData, Action> _delegates;

        public CarSwitcher(Car car, IEnumerable<IGarageData> garages, Timer timer, GameObject wheelPrefab, Observable<CarConfig> currentCarConfig)
        {
            _car = car;
            _garages = garages;
            _timer = timer;

            _delegates = new Dictionary<IGarageData, Action>();

            foreach (IGarageData garage in _garages)
            {
                Action OnPassed = () =>
                {
                    if (_timer.CurrentTime >= garage.TimeCost)
                    {
                        if (currentCarConfig.Value == garage.CarConfig)
                            return;

                        currentCarConfig.Value = garage.CarConfig;
                        Debug.Log(currentCarConfig.Value.name);
                        _car.InitCar(garage.CarConfig,wheelPrefab);
                        _timer.OffsetTime(-garage.TimeCost);
                    }
                };

                garage.passed += OnPassed;

                _delegates.Add(garage,OnPassed);
            }
        }

        public void Dispose()
        {
            foreach (var keyValuePair in _delegates)
            {
                if (keyValuePair.Key!=null)
                    keyValuePair.Key.passed-=keyValuePair.Value;
            }
        }
    }
}
