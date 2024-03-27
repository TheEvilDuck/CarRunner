using TMPro;
using UnityEngine;

namespace Gameplay.Garages
{
    public class GarageView : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private TextMeshProUGUI _badConditionText;
        [SerializeField] private TextMeshProUGUI _goodConditionText;

        public void Init(IGarageData garageData)
        {
            _badConditionText.text = $"Time < {garageData.ComparsionTime} = {garageData.AdditionalTime}";
            _goodConditionText.text = $"Time >= {garageData.ComparsionTime} = ";

            _meshFilter.mesh = garageData.CarConfig.ModelOfCar;
            _meshRenderer.materials = garageData.CarConfig.Materials;
        }

    }
}
