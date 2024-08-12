using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Garages
{
    public class StatsFiller : MonoBehaviour
    {
        [SerializeField] private Color32 _goodDeltaColor;
        [SerializeField] private Color32 _badDeltaColor;
        [SerializeField] private Image _backGround;
        [SerializeField] private Image _baseFiller;
        [SerializeField] private Image _deltaFiller;

        public void Init(float normalizedBaseValue, float normalizedDelta)
        {
            if (normalizedDelta > 0)
            {
                _deltaFiller.color = _goodDeltaColor;
                _baseFiller.fillAmount = normalizedBaseValue;
                _deltaFiller.fillAmount = normalizedBaseValue + normalizedDelta;
            }
            else
            {
                _deltaFiller.color = _badDeltaColor;
                _baseFiller.fillAmount = normalizedBaseValue + normalizedDelta;
                _deltaFiller.fillAmount = normalizedBaseValue;
            }
        }
    }
}
