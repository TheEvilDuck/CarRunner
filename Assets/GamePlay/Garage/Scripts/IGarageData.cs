using System;
using GamePlay.Cars.Scripts;

namespace GamePlay.Garage.Scripts
{
    public interface IGarageData
    {
        public event Action passed;
        public float TimeCost {get;}
        public CarConfig CarConfig {get;}
        
    }
}