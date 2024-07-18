using Common.UI.UIAnimations;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class CoinsView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private UITransparancyAnimator _appearAnimation;
        [SerializeField] private UINumberTextAnimator _addAnimation;

        private int _currentValue;

        private void OnEnable() 
        {
            _appearAnimation.StartAnimation();
        }

        public void UpdateValue(int newValue)
        {
            _addAnimation.ChangeTargetValue(_currentValue, newValue);
            _addAnimation.StartAnimation();
            _currentValue = newValue;
        }
    }
}
