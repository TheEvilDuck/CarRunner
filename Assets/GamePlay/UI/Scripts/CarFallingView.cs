using TMPro;
using UnityEngine;

namespace Gameplay.UI
{
    public class CarFallingView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;

        public void UpdateCount(int count) => _countText.text = count.ToString();
    }
}
