using Common;
using UnityEngine;

namespace Gameplay.Cars
{
    public class Car : MonoBehaviour, IPausable
    {
        [field: SerializeField]public CarBehaviour CarBehavior {get; private set;}

        private CarView _carView;

        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        public void InitCar(CarConfig config, GameObject wheelPrefab)
        {
            if (_carView != null)
                Destroy(_carView.gameObject);
            
            _carView = Instantiate(config.CarViewPrefab, transform);
            _carView.transform.localPosition = Vector3.zero;
            CarBehavior.ChangeWheelsOffsets(_carView.RFWheelPosition, _carView.LFWheelPosition, _carView.RBWheelPosition, _carView.LBWheelPosition);
            CarBehavior.Init(config.Acceleration, config.MaxSpeed);
            _carView.InitWheels(wheelPrefab, CarBehavior.Wheels);
            _carView.ChangeMaterial(config.Materials);
        }

        public void TeleportCar(Vector3 toPosition, Quaternion rotation)
        {
            CarBehavior.RemoveVelocity();

            CarBehavior.MovePosition(toPosition);
            CarBehavior.MoveRotation(rotation);
        }

        public void Pause()
        {
            CarBehavior.Pause();
            _carView.Pause();
        }

        public void Resume()
        {
            CarBehavior.Resume();
            _carView.Resume();
        }
    }
}
