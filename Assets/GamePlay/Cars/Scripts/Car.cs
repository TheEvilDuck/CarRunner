using UnityEngine;

namespace Gameplay.Cars
{
    public class Car : MonoBehaviour
    {
        [field: SerializeField]public CarBehaviour CarBehavior {get; private set;}
        [field: SerializeField]public CarView CarView {get; private set;}
        public void InitCar(CarConfig config, GameObject wheelPrefab)
        {
            CarBehavior.Init(config.Acceleration, config.MaxSpeed);
            CarView.InitWheels(wheelPrefab, CarBehavior.Wheels);
            CarView.ChangeModel(config.ModelOfCar, config.Materials);
        }
    }
}
