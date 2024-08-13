using Common.Reactive;
using Common.UI.UIAnimations;
using Gameplay.Cars;
using TMPro;
using UnityEngine;

namespace Gameplay.Garages
{
    public class GarageView : MonoBehaviour
    {
        [SerializeField] private Renderer _gateRenderer;
        [SerializeField] private TextMeshProUGUI _timeCostText;
        [SerializeField] private Transform _carViewPivot;
        [SerializeField] private Color32 _color;
        [SerializeField] private UIAnimatorSequence _passedAnimation;
        [SerializeField] private float _carRotationSpeed;
        [SerializeField] private StatsFiller _speedStatsFiller;
        [SerializeField] private StatsFiller _accelerationStatsFiller;
        private CarView _carView;
        private IGarageData _garageData;
        private IReadonlyObservable<CarConfig> _currentCarConfig;

        public void Init(IGarageData garageData, GameObject wheelPrefab, IReadonlyObservable<CarConfig> currentCarConfig)
        {
            _garageData = garageData;

            _timeCostText.text = $"-{_garageData.TimeCost}";
            _carView = Instantiate(_garageData.CarConfig.CarViewPrefab, _carViewPivot);
            
            var wheel_1 = Instantiate(wheelPrefab, _carView.RBWheelPosition, Quaternion.identity, _carView.transform);
            wheel_1.transform.localRotation = Quaternion.identity;

            var wheel_2 = Instantiate(wheelPrefab, _carView.RFWheelPosition, Quaternion.identity, _carView.transform);
            wheel_2.transform.localRotation = Quaternion.identity;

            var wheel_3 = Instantiate(wheelPrefab, _carView.LBWheelPosition, Quaternion.identity, _carView.transform);
            wheel_3.transform.localRotation = Quaternion.Euler(0, 180, 0);

            var wheel_4 = Instantiate(wheelPrefab, _carView.LFWheelPosition, Quaternion.identity, _carView.transform);
            wheel_4.transform.localRotation = Quaternion.Euler(0, 180, 0);

            _carView.enabled = false;
            _carView.transform.localPosition = Vector3.zero;
            _carView.transform.localScale = Vector3.one;
            _gateRenderer.material.SetColor("_TintColor", _color);

            _garageData.passed += OnGaragePassed;

            _currentCarConfig = currentCarConfig;
            _currentCarConfig.changed += OnPlayerCarChanged;
            OnPlayerCarChanged(_currentCarConfig.Value);
        }

        private void OnPlayerCarChanged(CarConfig carConfig)
        {
            if (carConfig == null)
                return;
                
            _speedStatsFiller.Init(_garageData.CarConfig.MaxSpeed / CarConfig.MAX_SPEED, (_garageData.CarConfig.MaxSpeed - carConfig.MaxSpeed) / CarConfig.MAX_SPEED);
            _accelerationStatsFiller.Init(_garageData.CarConfig.Acceleration / CarConfig.MAX_ACCELERATION, (_garageData.CarConfig.Acceleration - carConfig.Acceleration) / CarConfig.MAX_ACCELERATION);
        }

        private void OnDestroy() 
        {
            if (_garageData != null)
            {
                _garageData.passed -= OnGaragePassed;
            }
            if (_currentCarConfig != null)
            {
                _currentCarConfig.changed -= OnPlayerCarChanged;
            }
        }

        private void Update() 
        {
            if (_garageData == null)
                return;

            _carViewPivot.Rotate(Vector3.up, _carRotationSpeed * Time.deltaTime);
        }

        private void OnGaragePassed()
        {
            _garageData.passed -= OnGaragePassed;
            _passedAnimation.StartSequence();
            _gateRenderer.gameObject.SetActive(false);
        }

    }
}
