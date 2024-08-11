using UnityEngine;

namespace Common.UI.UIAnimations
{
    public class UIScaleAnimator : UIAnimator
    {
        [SerializeField] private RectTransform _target;
        [SerializeField] private Vector2 _goal;
        private Vector2 _startScale;
        protected override void SetupAnimation()
        {
            _startScale = _target.localScale;

            if (_inverse)
            {
                EvaluateAnimation(1f);
            }
        }
        protected override void EvaluateAnimation(float strength)
        {
            _target.localScale =  Vector2.Lerp(_startScale, _startScale + _goal, strength);
        }
    }
}
