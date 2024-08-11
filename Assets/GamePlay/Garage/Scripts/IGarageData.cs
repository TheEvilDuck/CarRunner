using System;
using Gameplay.Cars;

namespace Gameplay.Garages
{
    public interface IGarageData
    {
        public event Action passed;
        public float TimeCost {get;}
        public CarConfig CarConfig {get;}
        
    }
}