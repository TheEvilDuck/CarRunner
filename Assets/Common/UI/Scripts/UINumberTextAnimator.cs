using TMPro;
using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UINumberTextAnimator : UIAnimator
    {
        [SerializeField] private int _startValue;
        [SerializeField] private int _targetValue;
        [SerializeField] private string preText;
        [SerializeField] private string postText;
        [SerializeField] private TextMeshProUGUI _targetText;

        public void ChangeTargetValue(int targetValue)
        {
            _targetValue = targetValue;
        }
        protected override void EvaluateAnimation(float strength)
        {
            _targetText.text = preText + Mathf.FloorToInt(_startValue + (_targetValue - _startValue) * strength).ToString() + postText;
        }
    }
}
