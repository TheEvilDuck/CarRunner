using Common;
using UnityEngine;

namespace Gameplay.Cars
{
    public class Car : MonoBehaviour, IPausable
    {
        [field: SerializeField]public CarBehaviour CarBehavior {get; private set;}
        [field: SerializeField]public CarView CarView {get; private set;}

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        public void InitCar(CarConfig config, GameObject wheelPrefab)
        {
            CarBehavior.Init(config.Acceleration, config.MaxSpeed);
            CarView.InitWheels(wheelPrefab, CarBehavior.Wheels);
            CarView.ChangeModel(config.ModelOfCar, config.Materials);
        }

        public void TeleportCar(Vector3 toPosition, Quaternion rotation)
        {
            transform.position = toPosition;
            transform.rotation = rotation;

            CarBehavior.RemoveVelocity();
        }

        public void Pause()
        {
            CarBehavior.Pause();
            CarView.Pause();
        }

        public void Resume()
        {
            CarBehavior.Resume();
            CarView.Resume();
        }
    }
}
