using Gameplay.Cars;
using TMPro;
using UnityEngine;

namespace Gameplay
{
    public class Speedometr : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        private CarBehaviour _carBehaviour;

        public void Init(CarBehaviour carBehaviour)
        {
            _carBehaviour = carBehaviour;
        }

        private void Update() 
        {
            if (_carBehaviour == null)
                return;

            _text.text = $"Speed: {_carBehaviour.CurrentSpeed}";
        }
    }

}