using System;
using System.Collections.Generic;
using Gameplay.Cars;
using Gameplay.Garages;
using UnityEngine;

namespace Gameplay
{
    public class CarSwitcher: IDisposable
    {
        private Car _car;
        private Garage[] _garages;
        private Timer _timer;

        private Dictionary<Garage, Action> _delegates;

        public CarSwitcher(Car car, Garage[] garages, Timer timer, GameObject wheelPrefab)
        {
            _car = car;
            _garages = garages;
            _timer = timer;

            _delegates = new Dictionary<Garage, Action>();

            foreach (Garage garage in _garages)
            {
                Action OnPassed = () =>
                {
                    if (_timer.CurrentTime >= garage.TimeCost)
                    {
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
