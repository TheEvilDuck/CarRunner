using Gameplay.Cars;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class Speedometr : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Transform _arrow;
        [SerializeField, Min(0)] private float _maxSpeed;
        [SerializeField, Range(0, 360f)] private float _minArrowAngle;
        [SerializeField, Range(0, 360f)] private float _maxArrowAngle;
        [SerializeField] private float _angleOffset = -180f;
        private CarBehaviour _carBehaviour;

        public void Init(CarBehaviour carBehaviour)
        {
            _carBehaviour = carBehaviour;
        }

        private void OnValidate() 
        {
            _maxArrowAngle = Mathf.Max(_minArrowAngle, _maxArrowAngle);
        }

        private void Update() 
        {
            if (_carBehaviour == null)
                return;

            _text.text = Mathf.Floor(_carBehaviour.CurrentSpeed).ToString();

            float relativeSpeed = _carBehaviour.CurrentSpeed / _maxSpeed;
            float angle = -(_minArrowAngle + relativeSpeed * _maxArrowAngle + _angleOffset);
            _arrow.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

}