using TMPro;
using UnityEngine;

namespace Gameplay.Garages
{
    public class GarageView : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private TextMeshProUGUI _timeCostText;

        public void Init(IGarageData garageData)
        {
            _timeCostText.text = $"Time cost:\n{garageData.TimeCost}";

            _meshFilter.mesh = garageData.CarConfig.ModelOfCar;
            _meshRenderer.materials = garageData.CarConfig.Materials;
        }

    }
}
