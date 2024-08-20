using TMPro;
using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UINumberTextAnimator : UIAnimator
    {
        [SerializeField] private int _startValue;
        [SerializeField] private int _targetValue;
        [SerializeField] private TextMeshProUGUI _targetText;

        public void ChangeTargetValue(int startValue, int targetValue)
        {
            _startValue = startValue;
            _targetValue = targetValue;
        }
        protected override void EvaluateAnimation(float strength)
        {
            _targetText.text = Mathf.FloorToInt(_startValue + (_targetValue - _startValue) * strength).ToString();
        }
    }
}
